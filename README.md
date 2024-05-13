# VideoManagerApi
API for Products and Videos with Caching
This project provides a Web API for managing products and their associated videos. It utilizes ASP.NET Core and implements caching to improve performance.

Features:

CRUD operations (Create, Read, Update, Delete) for products and videos.
Leverages distributed caching (Redis) to store frequently accessed product videos, reducing database load.
Optional client-side caching headers can be added for further optimization.

Technology Stack:

1) ASP.NET Core Web API
2) Entity Framework Core
3) Distributed caching

API Endpoints:

Products:
GET api/products: Get all products.
GET api/products/{productId}: Get a specific product by ID.
POST api/products: Create a new product.
PUT api/products/{productId}: Update a product.
DELETE api/products/{productId}: Delete a product.
Videos:
GET api/products/{productId}/videos: Get all videos associated with a product.
GET api/products/{productId}/videos/{videoId}: Get a specific video by ID.
POST api/products/{productId}/videos: Add a new video to a product.
DELETE api/products/{productId}/videos/{videoId}: Delete a video.
