# Global Shopping - Your Gateway to Authentic Japanese Culture and Lifestyle
Welcome to "Global Shopping," your premier online destination for everything authentically Japanese. Discover a meticulously curated selection of Japanese delights, from delectable food to exquisite fashion, all sourced with a keen eye for quality and authenticity. With our user-friendly web application, your journey into Japanese culture and craftsmanship begins here. Shop with confidence, explore our cultural insights, and immerse yourself in the beauty of Japan, all from the comfort of your screen. "Shop Japanese" - where Japan comes to you.
 ## link to the deployed site.
 [Shop Japanese](https://globalshopping.azurewebsites.net/)
## Product Offering
We offer a diverse range of Japanese products that encapsulate the essence of Japan's rich culture and lifestyle. Our catalog spans Japanese canned food, intricately designed furniture, stylish fashion choices, cutting-edge electronics, and even anime-related items. Whether you're seeking authentic ramen, unique home decor, fashionable clothing, or the latest in electronics, we aim to bring the very best of Japan to your doorstep. Immerse yourself in Japan's cultural tapestry and enrich your life through our handpicked collection.
## Structure/Database Schema for your store DB
![Image](https://camo.githubusercontent.com/21538cb25738e310b6e63ef242889f799a92e9e8e9303cdf994396c00fd4af91/68747470733a2f2f692e696d6775722e636f6d2f713354453642432e706e67)


## Database Schema
Our application employs a well-structured database schema, represented by a set of C# classes, to facilitate seamless e-commerce operations. Below, we outline the key tables and their relationships within the database:
1. **Cart Table (`Cart` Class):**
   - `Id`: Unique identifier for each shopping cart.
   - `UserId`: Associated user for the cart.
   - `TotalPrice`: Total price of all products in the cart.
   - `Count`: Number of products in the cart.
   - `cartProducts`: A link to products added to the cart.
2. **CartProduct Table (`CartProduct` Class):**
   - `CartId`: ID of the cart to which the product belongs.
   - `ProductId`: ID of the product added to the cart.
   - `cart`: A link to the corresponding `Cart`.
   - `product`: A link to the referenced product.
3. **Order Table (`Order` Class):**
   - `Id`: Unique identifier for each order.
   - `UserId`: User who placed the order.
   - `TotalPrice`: Total price of the order.
   - `FirstName`, `LastName`: Customer's name.
   - `Address`, `City`, `State`, `Zip`: Shipping information.
   - `Timestamp`: Order placement timestamp.
   - `orderProduct`: A link to products included in the order.
4. **OrderProduct Table (`OrderProduct` Class):**
   - `OrderId`: ID of the order to which the product belongs.
   - `ProductId`: ID of the product included in the order.
   - `order`: A link to the corresponding `Order`.
   - `product`: A link to the referenced product.
5. **Product Table (`Product` Class):**
   - `Id`: Unique identifier for each product.
   - `Name`: Name of the product.
   - `ImageURL`: Product image URL.
   - `Price`: Product price.
   - `Amount`: Quantity available.
   - `Description`: Product description.
   - `categoriesProducts`: A link to product categories.
Our database schema establishes clear relationships between shopping carts, products, orders, and categories, enabling efficient management of user shopping carts, order placement, and product categorization.
## Claims and Policies
**Claims Captured:**
1. **Product Authenticity**: Ensuring all products are genuinely Japanese.
2. **Product Quality**: Guaranteeing high-quality standards.
3. **Cultural Representation**: Providing culturally accurate items.
4. **Safety and Compliance**: Complying with safety and legal regulations.
**Policies Enforced:**
1. **Return and Refund Policy**: Clarifying conditions for returns and refunds.
2. **Cultural Sensitivity Policy**: Ensuring respectful cultural representation.
3. **Safety and Compliance Policies**: Adhering to safety standards.
4. **Customer Data Protection Policy**: Safeguarding customer data and privacy to build trust and maintain legal compliance, customer satisfaction, and a positive brand reputation.
4:47
## User Roles and Functionality
### Administrator
As an Administrator, you have full control over products and categories to maintain content integrity and enhance the user experience. You can:
- View lists of all products and categories.
- Add new products and categories.
- Modify existing product and category details.
- Delete products and categories, ensuring associated products are handled correctly.
### Editor
Editors assist in managing products and categories to ensure accuracy and relevance on the site. You can:
- View lists of all products and categories.
- Modify existing product details.
- Modify existing category details.
### User
As a User, you can browse products and categories, explore available items, and in the future:
- Add products to your shopping cart.
- Place orders for selected products.
- Make secure payments for your orders.
## Contributors
We extend our heartfelt gratitude to our dedicated team for their expertise, commitment, and collaborative spirit in bringing this application to life:
- **OdaiAl-Obaidat**
- **Abdul Rahman Al-Suwaiti**
Thank you for choosing "Global Shopping." Explore, experience, and embrace the wonders of Japan with us.
