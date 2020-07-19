using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.Helpers;
using IdentityServer4.Events;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public class EventContextException : Exception
    {
        public EventContextException() { }
        public EventContextException(string message) : base(message) { }

    }

    public class EventContext
    {
        private readonly DbAntibodyCareToKnowProContext _dbContext = null;
        private LoginProfile _userContext = null;
        private Event _event = null;

        public EventContext(DbAntibodyCareToKnowProContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public void Initialize(EventType eventType, LoginProfile currentLoginProfile, string eventNotes = null)
        {
            _userContext = currentLoginProfile;
            if (_event == null)
            {
                _event = new Event();
                _event.EventType = (int) eventType;
                _event.LoginProfileId = _userContext.LoginProfileId;
                _event.EventDateUtc = DateTime.UtcNow;
                _event.EventNotes = eventNotes;
               // _event = _dbContext.Event.Create();
                _dbContext.Event.Add(_event);
            }
        }

        public Event GetEvent(bool getEmptyEvent = false)
        {
            if (_event != null && _event.EventEntity.Any())
            {
                return _event;
            }

            if (getEmptyEvent && _event != null)
            {
                return _event;
            }

            return null;
        }

        public void AddEntity(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException($@"Entity is null");

            if (_event == null)
                throw new ApplicationException("Event not initialized");

            ActionType actionType = _dbContext.GetActionType(entity);
            var changedProperties = _dbContext.GetChangedProperties(entity);

            // only bother recording something if a property changed
            if (changedProperties.Count > 0)
            {
                var type = entity.GetType();


                var entry = _dbContext.Entry(entity);
                var primaryKey = entry.Metadata.FindPrimaryKey();
                var primaryKeyPropertyName = primaryKey.Properties.First().Name;



               // var primaryKeyPropertyName = _dbContext.GetKey(entity);
                object primaryKeyValue = entity.GetType().GetProperty(primaryKeyPropertyName)?.GetValue(entity, null);

                EventEntity evEntity = null;
                System.Reflection.PropertyInfo foreignKeyProperty = null;

                foreach (var existingEventEntity in _event.EventEntity)
                {
                    if (foreignKeyProperty == null)
                        foreignKeyProperty = existingEventEntity.GetType().GetProperty(primaryKeyPropertyName);

                    // if we cannot find a property in the EventEntity with the same name as the primary key of the entity
                    // then that means we cannot store it and we should inform the dev in the most intrusive way (as this should be picked up during testing
                    // and either a foreign key added to the EventEntity table or do not attempt to add that type of entity to the event history)
                    if (foreignKeyProperty == null)
                        throw new EventContextException($"Event entity does not have a foreign key for {primaryKeyPropertyName}");

                    var foreignKeyValue = foreignKeyProperty.GetValue(existingEventEntity, null);

                    if (foreignKeyValue != null && foreignKeyValue.Equals(primaryKeyValue))
                    {
                        evEntity = existingEventEntity;
                        break;
                    }
                }

                if (evEntity == null)
                {
                    evEntity = new EventEntity();
                    evEntity.GetType().GetProperty(primaryKeyPropertyName)?.SetValue(evEntity, primaryKeyValue);
                    evEntity.ActionType = (int) actionType;
                    evEntity.Position = _event.EventEntity.Count + 1;

                    _event.EventEntity.Add(evEntity);
                }

                foreach (var changedProperty in changedProperties)
                {
                    // do we already have the property?
                    EventEntityProperty eventEntityProperty = evEntity.EventEntityProperty.FirstOrDefault(x => x.PropertyName == changedProperty.PropertyName);

                    if (eventEntityProperty == null)
                    {
                        evEntity.EventEntityProperty.Add(changedProperty);
                    }
                    else
                    {
                        eventEntityProperty.NewValue = changedProperty.NewValue;
                    }
                }
            }
        }
    }
}
