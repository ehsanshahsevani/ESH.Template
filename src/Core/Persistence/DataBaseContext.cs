using Domain;
using ESH.SeedworkSystem.DatabaseContext;
using ESH.SeedworkSystem.Domain.SubSystem;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DatabaseContext :
	BaseDbContext<DatabaseContext>
{
	public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
	{
	}

	public DbSet<Profile> Profiles { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<FieldType> FieldTypes { get; set; }
	public DbSet<CategoryType> CategoryTypes { get; set; }

	public DbSet<Field> Fields { get; set; }
	public DbSet<FieldMultiValue> FieldMultiValue { get; set; }
	public DbSet<FieldValueAnnouncement> FieldValueAnnouncements { get; set; }

	public DbSet<Status> Statuses { get; set; }
	public DbSet<Announcement> Announcements { get; set; }

	public DbSet<Note> Notes { get; set; }
	public DbSet<Favorite> Favorites { get; set; }

	public DbSet<DeleteReason> DeleteReasons { get; set; }

	public DbSet<NeedToEditLog> NeedToEditLogs { get; set; }
	public DbSet<NeedToEditReason> NeedToEditReasons { get; set; }

	public DbSet<ReportLog> ReportLogs { get; set; }
	public DbSet<ReportReason> ReportReasons { get; set; }

	public DbSet<DictionaryChecker> DictionaryCheckers { get; set; }

	public DbSet<Region> Regions { get; set; }
	public DbSet<PhoneOperator> PhoneOperators { get; set; }

	public DbSet<PlateCode> PlateCodes { get; set; }

	public DbSet<PlateStatus> PlateStatusList { get; set; }

	public DbSet<AnnouncementViews> AnnouncementViews { get; set; }

	public DbSet<ContactUs> ListContactUs { get; set; }
	public DbSet<CaptchaHistory> CapcCaptchaHistories { get; set; }
	
	#region OnConfiguring

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// optionsBuilder.UseLazyLoadingProxies();
	}

	#endregion /OnConfiguring

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<AnnouncementViews>(e =>
		{
			e.HasIndex(p => p.CreateDateTime);
			e.HasIndex(p => p.AnnouncementId);
		});

		modelBuilder.Entity<SubSystem>()
			.HasIndex(x => x.Name)
			.IsUnique(unique: true);

		// announcement -> price with default value 0
		modelBuilder.Entity<Announcement>()
			.Property(x => x.Price)
			.HasDefaultValue(value: 0);

		// profile -> bookmarks
		modelBuilder.Entity<Profile>()
			.HasMany(p => p.BookMarks)
			.WithOne(p => p.Profile)
			.HasForeignKey(m => m.ProfileId)
			.OnDelete(DeleteBehavior.NoAction);

		// profile -> notes
		modelBuilder.Entity<Profile>()
			.HasMany(p => p.Notes)
			.WithOne(p => p.Profile)
			.HasForeignKey(m => m.ProfileId)
			.OnDelete(DeleteBehavior.NoAction);

		// profile -> announcements
		modelBuilder.Entity<Profile>()
			.HasMany(p => p.Announcements)
			.WithOne(p => p.Profile)
			.HasForeignKey(m => m.ProfileId)
			.OnDelete(DeleteBehavior.NoAction);

		// profile -> need to edit logs
		modelBuilder.Entity<Profile>()
			.HasMany(p => p.NeedToEditLogs)
			.WithOne(p => p.Profile)
			.HasForeignKey(m => m.ProfileId)
			.OnDelete(DeleteBehavior.NoAction);

		// profile -> language code
		modelBuilder.Entity<Profile>()
			.HasOne(p => p.LanguageCode)
			.WithMany()
			.HasForeignKey(m => m.LanguageCodeId)
			.OnDelete(DeleteBehavior.NoAction);

		// profile -> report logs
		modelBuilder.Entity<Profile>()
			.HasMany(p => p.ReportLogs)
			.WithOne(p => p.Profile)
			.HasForeignKey(m => m.ProfileId)
			.OnDelete(DeleteBehavior.NoAction);

		// profiel -> announcmeent views
		modelBuilder.Entity<Profile>()
			.HasMany(p => p.AnnouncementViews)
			.WithOne(p => p.Profile)
			.HasForeignKey(m => m.ProfileId)
			.OnDelete(DeleteBehavior.NoAction);

		// caategory type -> categories
		modelBuilder.Entity<CategoryType>()
			.HasMany(p => p.Categories)
			.WithOne(p => p.CategoryType)
			.HasForeignKey(m => m.CategoryTypeId)
			.OnDelete(DeleteBehavior.NoAction);

		// category -> cildren
		modelBuilder.Entity<Category>()
			.HasMany(p => p.Children)
			.WithOne(p => p.Parent)
			.HasForeignKey(m => m.ParentId)
			.OnDelete(DeleteBehavior.NoAction);

		// feild -> feild multi values
		modelBuilder.Entity<Field>()
			.HasMany(p => p.FieldMultiValues)
			.WithOne(p => p.Field)
			.HasForeignKey(m => m.FieldId)
			.OnDelete(DeleteBehavior.NoAction);

		// feild -> category
		modelBuilder.Entity<Field>()
			.HasOne(p => p.Category)
			.WithMany(p => p.Feilds)
			.HasForeignKey(m => m.CategoryId)
			.OnDelete(DeleteBehavior.NoAction);

		// feilds -> feild type
		modelBuilder.Entity<Field>()
			.HasOne(p => p.FieldType)
			.WithMany(p => p.Fields)
			.HasForeignKey(m => m.FieldTypeId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcement -> favorites
		modelBuilder.Entity<Announcement>()
			.HasMany(current => current.Favorites)
			.WithOne(current => current.Announcement)
			.HasForeignKey(current => current.AnnouncementId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcement -> notes
		modelBuilder.Entity<Announcement>()
			.HasMany(current => current.Notes)
			.WithOne(current => current.Announcement)
			.HasForeignKey(current => current.AnnouncementId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcements -> status
		modelBuilder.Entity<Announcement>()
			.HasOne(current => current.Status)
			.WithMany(current => current.Announcements)
			.HasForeignKey(current => current.StatusId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcements -> dictionary checker
		modelBuilder.Entity<Announcement>()
			.HasOne(current => current.DictionaryChecker)
			.WithMany(current => current.Announcements)
			.HasForeignKey(current => current.DictionaryCheckerId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcements -> delete reason
		modelBuilder.Entity<Announcement>()
			.HasOne(current => current.DeleteReason)
			.WithMany(current => current.Announcements)
			.HasForeignKey(current => current.DeleteReasonId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcement -> need to edit logs
		modelBuilder.Entity<Announcement>()
			.HasMany(current => current.NeedToEditLogs)
			.WithOne(current => current.Announcement)
			.HasForeignKey(current => current.AnnouncementId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcements -> announcement views
		modelBuilder.Entity<Announcement>()
			.HasMany(current => current.AnnouncementViews)
			.WithOne(current => current.Announcement)
			.HasForeignKey(current => current.AnnouncementId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcements -> report logs
		modelBuilder.Entity<Announcement>()
			.HasMany(current => current.ReportLogs)
			.WithOne(current => current.Announcement)
			.HasForeignKey(current => current.AnnouncementId)
			.OnDelete(DeleteBehavior.NoAction);

		// announcement -> feild value announcements
		modelBuilder.Entity<FieldValueAnnouncement>()
			.HasOne(current => current.Announcement)
			.WithMany(current => current.FieldValueAnnouncements)
			.HasForeignKey(current => current.AnnouncementId)
			.OnDelete(DeleteBehavior.NoAction);

		// feild value announcements -> feild
		modelBuilder.Entity<FieldValueAnnouncement>()
			.HasOne(current => current.Field)
			.WithMany(current => current.FieldValueAnnouncements)
			.HasForeignKey(current => current.FieldId)
			.OnDelete(DeleteBehavior.NoAction);

		// report reason -> report logs
		modelBuilder.Entity<ReportReason>()
			.HasMany(current => current.ReportLogs)
			.WithOne(current => current.ReportReason)
			.HasForeignKey(current => current.ReportReasonId)
			.OnDelete(DeleteBehavior.NoAction);

		// need to edit reason -> need to edit logs
		modelBuilder.Entity<NeedToEditReason>()
			.HasMany(current => current.NeedToEditLogs)
			.WithOne(current => current.NeedToEditReason)
			.HasForeignKey(current => current.NeedToEditReasonId)
			.OnDelete(DeleteBehavior.NoAction);

		// region to region list
		modelBuilder.Entity<Region>()
			.HasOne(r => r.Parent)
			.WithMany(r => r.Regions)
			.HasForeignKey(r => r.ParentId)
			.OnDelete(DeleteBehavior.NoAction);

		base.OnModelCreating(modelBuilder);
	}
}