namespace ThaiTable.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FoodContext : DbContext
    {
        public FoodContext()
            : base("name=FoodContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<ModifierOfOrder> ModifierOfOrders { get; set; }
        public virtual DbSet<Modifier> Modifiers { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ShopTimeSlot> ShopTimeSlots { get; set; }
        public virtual DbSet<WeekTimeSlot> WeekTimeSlots { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
    .HasMany(e => e.Foods)
    .WithOptional(e => e.Category)
    .HasForeignKey(e => e.Category_CategoryID);

            modelBuilder.Entity<OrderItem>()
                .Property(e => e.MoveToClient)
                .IsFixedLength();

            modelBuilder.Entity<OrderItem>()
                .HasMany(e => e.ModifierOfOrders)
                .WithRequired(e => e.OrderItem)
                .HasForeignKey(e => new { e.OrderID, e.ProductID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.MoveToClient)
                .IsFixedLength();

            modelBuilder.Entity<Order>()
    .HasOptional(e => e.Customer)
    .WithRequired(e => e.Order);

            modelBuilder.Entity<Order>()
                .Property(e => e.FeeShipping);
                //.HasPrecision(18, 0);

            modelBuilder.Entity<WeekTimeSlot>()
                .Property(e => e.OrderType)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
