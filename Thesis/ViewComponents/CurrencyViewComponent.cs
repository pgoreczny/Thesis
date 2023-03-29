using Microsoft.AspNetCore.Mvc;
using Thesis.Services;

namespace Thesis.ViewComponents
{
    public class CurrencyViewComponent : ViewComponent
    {
        private readonly PaymentService paymentService;
        public CurrencyViewComponent(PaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return await Task.FromResult((IViewComponentResult)View("Currency", paymentService.getMainCurrency()));
        }
    }
}
