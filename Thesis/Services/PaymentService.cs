using Thesis.database;

namespace Thesis.Services
{
    public class PaymentService
    {
        private readonly CoursesDBContext context;

        public PaymentService(CoursesDBContext context)
        {
            this.context = context;
        }

        public string getMainCurrency()
        {
            return context.currencies
                .Where(currency => currency.isDefault)
                .FirstOrDefault()
                .abbreviation;
        }
    }
}
