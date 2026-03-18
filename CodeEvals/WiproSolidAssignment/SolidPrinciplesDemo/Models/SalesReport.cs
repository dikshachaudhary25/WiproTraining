namespace SolidPrinciplesDemo.Models
{
    public class SalesReport : Report
    {
        public override string Generate()
        {
            return "Sales Report Content";
        }
    }
}