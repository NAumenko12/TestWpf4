//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Заказ
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Заказ()
        {
            this.Состав_Заказа = new HashSet<Состав_Заказа>();
        }
    
        public int Id_Заказа { get; set; }
        public Nullable<int> Офицанта { get; set; }
        public Nullable<int> Стол { get; set; }
    
        public virtual Сотрудники Сотрудники { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Состав_Заказа> Состав_Заказа { get; set; }
    }
}
