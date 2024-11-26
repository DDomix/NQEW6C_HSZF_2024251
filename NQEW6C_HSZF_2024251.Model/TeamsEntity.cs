using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NQEW6C_HSZF_2024251.Model
{
    public class TeamsEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string TeamName { get; set; }
        public int Year { get; set; }
        public string HeadQuarters { get; set; }
        public string TeamPrincipal { get; set; }
        public int ConstructorsChampionshipWins { get; set; }
        public int BudgetId { get; set; }

        
        [ForeignKey("BudgetId")]
        public virtual Budget Budget { get; set; }

        
        public TeamsEntity()
        {
            
        }
    }

    public class Budget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TotalBudget { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }

        public Budget()
        {
            Expenses = new HashSet<Expense>();
        }
    }

    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public string ApprovalStatus { get; set; }
        public DateTime ExpenseDate { get; set; }

        public int BudgetId { get; set; }

        [ForeignKey("BudgetId")]
        public virtual Budget Budget { get; set; }
        public virtual ICollection<SubCategory> SubCategory { get; set; }

        public Expense()
        {
            SubCategory = new HashSet<SubCategory>(); 
        }
    }

    public class SubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }

        public SubCategory() { }
    }

}

