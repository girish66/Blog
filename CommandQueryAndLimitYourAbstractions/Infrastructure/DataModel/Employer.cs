//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infrastructure.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employer
    {
        public Employer()
        {
            this.Employees = new HashSet<Employee>();
        }
    
        public string EmployerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
