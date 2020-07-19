using System;
using System.Collections.Generic;
using System.Linq;
using Antibody.CareToKnowPro.CRM.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Antibody.CareToKnowPro.CRM.Models
{
    public partial class DbAntibodyCareToKnowProContext : DbContext
    {
        public DbAntibodyCareToKnowProContext()
        {
        }

        public DbAntibodyCareToKnowProContext(DbContextOptions<DbAntibodyCareToKnowProContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContactReasons> ContactReasons { get; set; }
        public virtual DbSet<ContactUsForms> ContactUsForms { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventEntity> EventEntity { get; set; }
        public virtual DbSet<EventEntityProperty> EventEntityProperty { get; set; }
        public virtual DbSet<LoginProfile> LoginProfile { get; set; }
        public virtual DbSet<LoginProfileRole> LoginProfileRole { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Specialty> Specialty { get; set; }
        public virtual DbSet<SpecialtyGroup> SpecialtyGroup { get; set; }
        public virtual DbSet<UnsubscribeReasons> UnsubscribeReasons { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserSpecialty> UserSpecialty { get; set; }
        public virtual DbSet<UserToken> UserToken { get; set; }
        public virtual DbSet<UserUnsubscribe> UserUnsubscribe { get; set; }

        public ActionType GetActionType(object entity)
        {
            var entry = ChangeTracker.Entries().FirstOrDefault(x => x.Entity == entity);
            ActionType actionType = ActionType.Unknown;

            if (entry != null)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        actionType = ActionType.Added;
                        break;

                    case EntityState.Deleted:
                        actionType = ActionType.Deleted;
                        break;

                    case EntityState.Modified:
                        actionType = ActionType.Modified;
                        break;
                }
            }

            return actionType;
        }

        public List<EventEntityProperty> GetChangedProperties(object entity)
        {
            List<EventEntityProperty> changedProperties = new List<EventEntityProperty>();

            var entry = ChangeTracker.Entries().FirstOrDefault(x => x.Entity == entity);

            if (entry != null)
            {
                if (entry.State == EntityState.Added)
                {
                    var currentValues = entry.CurrentValues;

                    foreach (var propName in currentValues.Properties.Where(a=> a.Name != "Notes"))
                    {
                        string newValue = null;

                        if (currentValues[propName] != null)
                        {
                            var propertyInfo = entry.Entity.GetType().GetProperties().First(x => x.Name == propName.Name);

                            if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                            {
                                newValue = ((DateTime)currentValues[propName]).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                            }
                            else
                            {
                                newValue = currentValues[propName].ToString();
                            }
                        }

                        EventEntityProperty eventEntityProperty = new EventEntityProperty();
                        eventEntityProperty.PropertyName = propName.Name;
                        eventEntityProperty.OriginalValue = null;
                        eventEntityProperty.NewValue = newValue;

                        changedProperties.Add(eventEntityProperty);
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    var currentValues = entry.CurrentValues;
                    var originalValues = entry.OriginalValues;

                    foreach (var propName in originalValues.Properties.Where(a => a.Name != "Notes"))
                    {
                        string originalValue = null;
                        string newValue = null;

                        var propertyInfo = entry.Entity.GetType().GetProperties().First(x => x.Name == propName.Name);

                        if (originalValues[propName] != null)
                        {
                            if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                            {
                                originalValue = ((DateTime)originalValues[propName]).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                            }
                            else
                            {
                                originalValue = originalValues[propName].ToString();
                            }
                        }

                        if (currentValues[propName] != null)
                        {
                            if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                            {
                                newValue = ((DateTime)currentValues[propName]).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                            }
                            else
                            {
                                newValue = currentValues[propName].ToString();
                            }
                        }

                        if (originalValue == newValue)
                        {
                            continue;
                        }

                        EventEntityProperty eventEntityProperty = new EventEntityProperty();
                        eventEntityProperty.PropertyName = propName.Name;
                        eventEntityProperty.OriginalValue = originalValue;
                        eventEntityProperty.NewValue = newValue;

                        changedProperties.Add(eventEntityProperty);
                    }
                }
            }

            return changedProperties;
        }

        public virtual string GetKey<T>(T entity)
        {
            var keyName = this.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

            return keyName;
            //return (int)entity.GetType().GetProperty(keyName).GetValue(entity, null);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ContactReasons>(entity =>
            {
                entity.Property(e => e.ReasonId).ValueGeneratedNever();
            });

            modelBuilder.Entity<ContactUsForms>(entity =>
            {
                entity.Property(e => e.ContactId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasOne(d => d.LoginProfile)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.LoginProfileId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Event_Login");
            });

            modelBuilder.Entity<EventEntity>(entity =>
            {
                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventEntity)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EventEntity_Event");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EventEntity)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EventEntity_User");
            });

            modelBuilder.Entity<EventEntityProperty>(entity =>
            {
                entity.Property(e => e.PropertyName).IsUnicode(false);

                entity.HasOne(d => d.EventEntity)
                    .WithMany(p => p.EventEntityProperty)
                    .HasForeignKey(d => d.EventEntityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EventEntityProperty_EventEntity");
            });

            modelBuilder.Entity<LoginProfile>(entity =>
            {
                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.CompanyName).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.EmailConfirmed).HasDefaultValueSql("((1))");

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.Postal).IsUnicode(false);

                entity.Property(e => e.ProfileAnswer).IsUnicode(false);

                entity.Property(e => e.ProfileQuestion).IsUnicode(false);

                entity.Property(e => e.ProvCode).IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Street1).IsUnicode(false);
            });

            modelBuilder.Entity<LoginProfileRole>(entity =>
            {
                entity.HasOne(d => d.LoginProfile)
                    .WithMany(p => p.LoginProfileRole)
                    .HasForeignKey(d => d.LoginProfileId)
                    .HasConstraintName("FK_LoginProfileRole_LoginProfile1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.LoginProfileRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_LoginProfileRole_Role1");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.Property(e => e.ProvinceId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Permission).IsUnicode(false);
            });

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.Property(e => e.SpecialtyId).ValueGeneratedNever();

                entity.HasOne(d => d.SpecialtyGroup)
                    .WithMany(p => p.Specialty)
                    .HasForeignKey(d => d.SpecialtyGroupId)
                    .HasConstraintName("FK_Specialty_SpecialtyGroup");
            });

            modelBuilder.Entity<SpecialtyGroup>(entity =>
            {
                entity.Property(e => e.SpecialtyGroupId).ValueGeneratedNever();
            });

            modelBuilder.Entity<UnsubscribeReasons>(entity =>
            {
                entity.Property(e => e.ReasonId).ValueGeneratedNever();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedOnAdd();

                entity.Property(e => e.PreferredLanguage).IsUnicode(false);

                entity.HasOne(d => d.Province);
                //.WithMany(p => p.User)
                //.HasForeignKey(d => d.ProvinceId)
                //.HasConstraintName("FK_User_Province");
            });

            modelBuilder.Entity<UserSpecialty>(entity =>
            {
                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.UserSpecialty)
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpecialty_Specialty");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSpecialty)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserSpecialty_User");
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserToken)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserToken_User");
            });

            modelBuilder.Entity<UserUnsubscribe>(entity =>
            {
                entity.HasOne(d => d.Reason)
                    .WithMany(p => p.UserUnsubscribe)
                    .HasForeignKey(d => d.ReasonId)
                    .HasConstraintName("FK_UserUnsubscribe_UnsubscribeReasons");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserUnsubscribe)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserUnsubscribe_User");
            });
        }
    }
}
