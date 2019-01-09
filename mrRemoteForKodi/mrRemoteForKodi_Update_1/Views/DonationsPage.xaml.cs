using System;
using Windows.ApplicationModel.Store;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace mrRemoteForKodi_Update_1.Views
{
    public sealed partial class DonationsPage : Page
    {
        public DonationsPage()
        {
            InitializeComponent();
        }

        private void buttonDonate099_Click(object sender, RoutedEventArgs e)
        {
            textBlockPurchase.Text = "";
            PurchaseAddon("Donate_099");
        }

        private void buttonDonate499_Click(object sender, RoutedEventArgs e)
        {
            textBlockPurchase.Text = "";
            PurchaseAddon("Donate_499");
        }

        private void buttonDonate999_Click(object sender, RoutedEventArgs e)
        {
            textBlockPurchase.Text = "";
            PurchaseAddon("Donate_999");
        }

        private async void PurchaseAddon(string storeId)
        {
            try
            {
                PurchaseResults purchaseResults = await CurrentApp.RequestProductPurchaseAsync(storeId);
                switch (purchaseResults.Status)
                {
                    case ProductPurchaseStatus.Succeeded:
                        FulfillProduct(storeId, purchaseResults.TransactionId);
                        break;
                    case ProductPurchaseStatus.NotFulfilled:
                        FulfillProduct(storeId, purchaseResults.TransactionId);
                        break;
                    case ProductPurchaseStatus.NotPurchased:
                        textBlockPurchase.Text = "Product was not purchased.";
                        break;
                }
            }
            catch
            {
                textBlockPurchase.Text = "Unable to buy product";
            }
        }

        private async void FulfillProduct(string storeId, Guid transactionId)
        {
            try
            {
                FulfillmentResult result = await CurrentApp.ReportConsumableFulfillmentAsync(storeId, transactionId);
                switch (result)
                {
                    case FulfillmentResult.Succeeded:
                        textBlockPurchase.Text = "You bought and fulfilled product";
                        break;
                    case FulfillmentResult.NothingToFulfill:
                        textBlockPurchase.Text = "There is no purchased product to fulfill.";
                        break;
                    case FulfillmentResult.PurchasePending:
                        textBlockPurchase.Text = "You bought product. The purchase is pending so we cannot fulfill the product.";
                        break;
                    case FulfillmentResult.PurchaseReverted:
                        textBlockPurchase.Text = "You bought product, but your purchase has been reverted.";
                        // Since the user's purchase was revoked, they got their money back.
                        // You may want to revoke the user's access to the consumable content that was granted.
                        break;
                    case FulfillmentResult.ServerError:
                        textBlockPurchase.Text = "You bought product. There was an error when fulfilling.";
                        break;
                }
            }
            catch
            {
                textBlockPurchase.Text = "You bought product. There was an error when fulfilling.";
            }
        }
    }
}