using Microsoft.AspNetCore.Mvc;
using RapsAssessment_ShoppingCart.Models;

namespace RapsAssessment_ShoppingCart.Controllers
{
    public class ShoppingCartController : Controller
    {
        // Static cart to hold items added by the user during their session
         public static List<CartItem> cart = new List<CartItem>();

        // Static variables to store the current discount total and applied coupon code
        public static decimal? discountedTotal = null;
        public static string? appliedCouponCode = null;

        // Dictionary to hold available coupon codes and their respective discount percentages
        private static readonly Dictionary<string, decimal> couponDiscounts = new Dictionary<string, decimal>
        {
            { "SAVE10", 10 }, // 10% discount
            { "SAVE20", 20 }, // 20% discount
            { "SAVE30", 30 }  // 30% discount
        };

        // Displays the contents of the cart with any applied discount
        public IActionResult ViewCart()
        {
            RecalculateDiscount(); // Recalculate the discount before displaying the cart
            ViewBag.DiscountedTotal = discountedTotal; // Pass discounted total to the view
            ViewBag.CouponMessage = TempData["CouponMessage"] as string; // Retrieve coupon message from TempData
            return View(cart); // Pass the cart items to the view
        }

        // Displays available products
        public IActionResult Product()
        {
            var products = ProductData.GetProducts();
            return View(products);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, string productName, decimal unitPrice)   // Adds a product to the cart
        {
            // Check if the item is already in the cart
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                // Increment quantity and update the total price for the item
                item.Quantity++;
                item.TotalPrice = unitPrice * item.Quantity;
            }
            else
            {
                // Add new item to cart with initial quantity of 1 and calculate its total price
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    UnitPrice = unitPrice,
                    Quantity = 1,
                    TotalPrice = unitPrice
                });
            }

            RecalculateDiscount(); // Recalculate discount after adding item
            return RedirectToAction("ViewCart"); // Redirect to ViewCart to display updated cart
        }
        
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)  // Removes an item from the cart by productId
        {
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {               
                cart.Remove(item);   // Remove the item from the cart if it exists
            }
            RecalculateDiscount(); // Recalculate discount after item removal
            return RedirectToAction("ViewCart"); // Redirect to updated cart view
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int productId)   // Increases the quantity of an item in the cart by productId
        {
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                // Increase item quantity and update total price
                item.Quantity++;
                item.TotalPrice = item.UnitPrice * item.Quantity;
            }
            RecalculateDiscount(); // Recalculate discount after quantity increase
            return RedirectToAction("ViewCart"); // Redirect to updated cart view
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int productId)    // Decreases the quantity of an item in the cart, removes if quantity reaches zero
        {
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    // Decrease item quantity and update total price
                    item.Quantity--;
                    item.TotalPrice = item.UnitPrice * item.Quantity;
                }
                else
                { 
                    cart.Remove(item);  // Remove the item from the cart if quantity is 1 and decreases
                }
            }
            RecalculateDiscount(); // Recalculate discount after quantity decrease
            return RedirectToAction("ViewCart"); // Redirect to updated cart view
        }

        [HttpPost]
        public IActionResult ApplyDiscount(string couponCode)  // Applies a discount if the provided coupon code is valid
        {
            // Check if coupon code is valid and retrieve discount percentage
            if (couponCode != null && couponDiscounts.TryGetValue(couponCode.ToUpper(), out decimal discountPercentage))
            {
                appliedCouponCode = couponCode.ToUpper(); // Store the applied coupon code
            }
            else
            {
                // Invalid coupon, clear discount and set an error message
                discountedTotal = null;
                appliedCouponCode = null;
                TempData["CouponMessage"] = "Please select a valid coupon.";
            }
            RecalculateDiscount(); // Recalculate discount after applying coupon
            return RedirectToAction("ViewCart"); // Redirect to cart view to show any updated discount or error message
        }
        public void RecalculateDiscount()  // Calculates the total discount based on the applied coupon code
        {
            decimal total = cart.Sum(i => i.TotalPrice);  // Calculate total price of items in the cart

            if (appliedCouponCode != null && couponDiscounts.TryGetValue(appliedCouponCode, out decimal discountPercentage))
            {
                // Calculate discount amount and apply it to total
                decimal discountAmount = total * (discountPercentage / 100);
                discountedTotal = total - discountAmount;

                // Set a message to show the coupon is applied
                if (discountedTotal > 0)
                {
                    TempData["CouponMessage"] = $"Coupon '{appliedCouponCode}' applied! You saved {discountPercentage}% on your order.";
                }
            }
            else
            {
                discountedTotal = null; // Reset the discount if no valid coupon is applied
            }
            ViewBag.DiscountedTotal = discountedTotal; // Update ViewBag for discounted total in the view
        }

        [HttpPost]
        public IActionResult RemoveCoupon()    // Removes the applied coupon, resetting the discount
        {
            discountedTotal = null; // Clear discounted total
            appliedCouponCode = null; // Clear applied coupon code

            return RedirectToAction("ViewCart"); // Redirect to updated cart view without discount
        }
    }
}
