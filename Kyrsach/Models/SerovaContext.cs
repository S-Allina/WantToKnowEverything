using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kyrsach.Models
{
    public partial class SerovaContext : IdentityDbContext<UserModel>
    {
        public SerovaContext() {}

        public SerovaContext(DbContextOptions<SerovaContext> options) : base(options) {}

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<AnswersUser> AnswersUsers { get; set; } = null!;
        public virtual DbSet<Category> Category { get; set; } = null!;
        public virtual DbSet<Pair> Pairs { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Quez> Quezs { get; set; } = null!;
        public virtual DbSet<Test> Tests { get; set; } = null!;
        public virtual DbSet<UserViewModel> UserView { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<FileModel> Files { get; set; } = null!;
        public virtual DbSet<PeopleInGroup> PeopleInGroups { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("workstation id=Serova4.mssql.somee.com;packet size=4096;user id=s_alliina_SQLLogin_1;pwd=w396xy5e76;data source=Serova4.mssql.somee.com;persist security info=False;initial catalog=Serova4;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.IdAnswer);

                entity.ToTable("Answer");

                entity.Property(e => e.Answer1)
                    .HasMaxLength(50)
                    .HasColumnName("Answer");

                entity.Property(e => e.Loyal)
                    .HasMaxLength(50)
                    .HasColumnName("loyal");

                entity.HasOne(d => d.IdAnswersUserNavigation)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.IdAnswersUser)
                    .HasConstraintName("FK_Answer_AnswersUser");

                entity.HasOne(d => d.IdQuestionNavigation)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.IdQuestion)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<AnswersUser>(entity =>
            {
                entity.HasKey(e => e.IdAnswersUser);

                entity.ToTable("AnswersUser");

                entity.Property(e => e.IdUser).HasMaxLength(450);

                entity.Property(e => e.Time).HasColumnType("datetime");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory)
                    .HasName("PK_Сategory");

                entity.ToTable("Category");

                entity.Property(e => e.NameCategory).HasMaxLength(50);

                entity.Property(e => e.WhoCreatedCategory).HasMaxLength(450);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Pair>(entity =>
            {
                entity.HasKey(e => e.IdPair);

                entity.ToTable("Pair");

                entity.Property(e => e.Card1Text).HasMaxLength(50);

                entity.Property(e => e.Card2Text).HasMaxLength(50);

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Pairs)
                    .HasForeignKey(d => d.IdCategory)
                    .HasConstraintName("FK_Pair_Category");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.IdQuestion);

                entity.ToTable("Question");

                entity.Property(e => e.CorrectAnswer).HasMaxLength(50);

                entity.Property(e => e.Option1).HasMaxLength(50);

                entity.Property(e => e.Option2).HasMaxLength(50);

                entity.Property(e => e.Option3).HasMaxLength(50);

                entity.Property(e => e.Option4).HasMaxLength(50);

                entity.Property(e => e.Option5).HasMaxLength(50);

                entity.HasOne(d => d.IdTestNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.IdTest)
                    .HasConstraintName("FK_Question_Test");
            });

            modelBuilder.Entity<Quez>(entity =>
            {
                entity.HasKey(e => e.IdQuez);

                entity.ToTable("Quez");

                entity.Property(e => e.Pic1).HasColumnName("pic1");

                entity.Property(e => e.Pic2).HasColumnName("pic2");

                entity.Property(e => e.Pic3).HasColumnName("pic3");

                entity.Property(e => e.Pic4).HasColumnName("pic4");

                entity.Property(e => e.Pic5).HasColumnName("pic5");

                entity.Property(e => e.Q1).HasMaxLength(50);

                entity.Property(e => e.Q2).HasMaxLength(50);

                entity.Property(e => e.Q3).HasMaxLength(50);

                entity.Property(e => e.Q4).HasMaxLength(50);

                entity.Property(e => e.Q5).HasMaxLength(50);

                entity.HasOne(d => d.IdTestNavigation)
                    .WithMany(p => p.Quezs)
                    .HasForeignKey(d => d.IdTest)
                    .HasConstraintName("FK_Quez_Test");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasKey(e => e.IdTest);

                entity.ToTable("Test");

                entity.Property(e => e.NameTest).HasMaxLength(50);

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.IdCategory)
                    .HasConstraintName("FK_Test_Сategory");
            });
            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
