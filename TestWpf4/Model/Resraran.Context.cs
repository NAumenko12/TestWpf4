﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWpf4.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class var4Entities : DbContext
    {
        public var4Entities()
            : base("name=var4Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Блюдо> Блюдо { get; set; }
        public virtual DbSet<Должности> Должности { get; set; }
        public virtual DbSet<Заказ> Заказ { get; set; }
        public virtual DbSet<Ингридиента> Ингридиента { get; set; }
        public virtual DbSet<Категория_блюда> Категория_блюда { get; set; }
        public virtual DbSet<Состав_Блюда> Состав_Блюда { get; set; }
        public virtual DbSet<Состав_Заказа> Состав_Заказа { get; set; }
        public virtual DbSet<Сотрудники> Сотрудники { get; set; }
    }
}