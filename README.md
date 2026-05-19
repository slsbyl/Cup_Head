# Fluffy Platform - System Requirements Specification (Extensions)

## 8.2 Class Descriptions 

### 8.2.1 Class: User 
- **Purpose**: Manages core identity and security. 
- **Collaborations**: Interacts with JWTService for sessions and Bcrypt for hasing. 
- **Attributes**: 
  - `name` (String): User's full name. Required. 
  - `email` (String): Unique identifier. Trimmed and lowercased. 
  - `password` (String): Hashed using Bcrypt (Select: false for query security). 
  - `role` (Enum): `['owner', 'admin', 'customer']`. Default: `'customer'`. 
  - `passwordResetToken` (String): SHA-256 hash of the plain reset token. 
  - `passwordResetExpires` (Date): Timestamp (Expires in 10 minutes). 
- **Operations**: 
  - `signup(data)`: Validates unique email and persists a new User instance. 
  - `login(email, password)`: Verifies credentials and returns a JWT (signed via signToken). 
  - `createPasswordResetToken()`: Generates random bytes, hashes them via crypto, and sets expiry. 
  - `correctPassword(candidate, hash)`: Asynchronous comparison of plain-text vs hashed passwords. Invokes bcrypt.compare to verify login. 
- **Constraints**: Email must be unique. Password is required and not returned in queries by default. 

### 8.2.2 Class: Product 
- **Abstract or Concrete**: Concrete. 
- **List of Superclasses/Subclasses**: None. 
- **Purpose**: Represents a retail item with inventory tracking and market perfmance data. 
- **Collaborations**: Interacts with RetailOrder (for stock sync) and Customer (for Try-On). 
- **Attributes**: 
  - `name` (String): Product title. Required. 
  - `price` (Number): Unit price. Required. 
  - `description` (String): Full product details. 
  - `image` (String): Primary display URL (Fallbacks to placeholder). 
  - `images` (Array[String]): Collection of image URLs for the gallery. 
  - `category` (String): e.g., 'Tops', 'Dresses'. 
  - `sizes` (Array[String]): Available size options. 
  - `colors` (Array[String]): Available color options (HEX or names). 
  - `stock` (Number): Inventory level. Default: 0, Min: 0. 
  - `isBestSeller` (Boolean): Flag for catalog highlighting. 
  - `isNewArrival` (Boolean): Flag for automated sorting. 
  - `rating` (Number): Average customer rating (Default: 4.5). 
  - `reviews` (Number): Total count of feedback entries. 
  - `soldCount` (Number): Incremented atomically on every purchase. 
- **Operations**: 
  - `hasStock(quantity)`: Returns Boolean. Validates `this.stock >= quantity`. 
  - `updateStock(decrement)`: Side Effect: Updates DB using `$inc` to adjust stock and soldCount. 
- **Constraints**: `stock` cannot drop below 0. `price` must be a positive number. 

### 8.2.3 Class: RetailOrder 
- **Purpose**: Encapsulates a Retail/Brand (B2C) transaction and its fulfillment sattus. 
- **Collaborations**: Interacts with Product for validation and Nodemailer for user alerts. 
- **Attributes**: 
  - `customerName` (String): Brand client's name. 
  - `phone` (String): Contact number for logistics. 
  - `address` (String): Full physical destination. 
  - `governorate` (String): Region for shipping calculation. 
  - `shippingFee` (Number): Default: 0. Derived from Settings. 
  - `items` (Array[Object]): Contains `productId`, `productName`, `color`, `size`, `quantity` (min: 1), `price`. 
  - `totalAmount` (Number): Automated calculation result. 
  - `status` (Enum): `['Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled']`. Default: `'Pending'`. 
  - `createdAt` (Date): Automated timestamp. 
- **Operations**: 
  - `pre-save hook`: Returns void. Side Effect: Reduces items array to calculate subtotal and adds shippingFee. 
  - `placeOrder(data)`: Validates stock availability for ALL items before persistence. 
- **Constraints**: `phone`, `address`, and `customerName` are required. `items` array cannot be empty. 

### 8.2.4 Class: FactoryClient 
- **Purpose**: Models a B2B partner (عميل المصنع) with its own private portal and financial ledger. 
- **Collaborations**: Interacts with WholesaleOrder to manage production debt and payments. 
- **Attributes**: 
  - `companyName` (String): Business name. Required. 
  - `ownerName` (String): Primary contact person. Required. 
  - `phone` (String): Contact details. Required. 
  - `username` (String): Unique login identifier. Required. 
  - `password` (String): Clear-text/hashed login (based on B2B portal logic). 
  - `totalDebt` (Number): Running total of balance due. Default: 0. 
  - `paidAmount` (Number): Historical sum of settlements. Default: 0. 
- **Operations**: 
  - `login(username, password)`: Queries database for matching B2B credentials. 
  - `recordPayment(amount)`: Returns void. Side Effect: Invokes `$inc` on `paidAmount` field. 
  - `getBalance()`: Returns the calculated current debt (`totalDebt - paidAmount`). 
- **Constraints**: `username` must be unique in the collection. 

### 8.2.5 Class: WholesaleOrder 
- **Purpose**: Manages a bulk production lifecycle from design to delivery. 
- **Collaborations**: Collaborates with FactoryClient for debt logic. 
- **Attributes**: 
  - `clientId` (ObjectId): Reference to the FactoryClient. Required. 
  - `productName` (String): Name of the design to be manufactured. Required. 
  - `productImage` (String): Primary reference sketch. 
  - `productImages` (Array[String]): Collection of technical designs. 
  - `details` (String): Fabric specs and manufacturing notes. 
  - `colors` (Array[String]): List of production colors. 
  - `totalQuantity` (Number): Calculated piece count. 
  - `pricePerPiece` (Number): Set by Owner after "Awaiting Pricing". Default: 0. 
  - `totalPrice` (Number): Default: 0. Calculated as `pricePerPiece * totalQuantity`. 
  - `status` (Enum): `['في انتظار التسعير', 'قيد الانتظار', 'جاري القص', 'جاري الخياطة', 'تم التسليم']`
  - `isDebtAdded` (Boolean): Security flag to ensure debt is added to FactoryClient exactly once. 
- **Operations**: 
  - `updateOrder(data)`: Side Effect: Re-calculates `totalPrice` if `pricePerPiece` or `totalQuantity` changes. 
  - `toggleDebtStatus()`: Side Effect: Increments or decrements `FactoryClient.totalDebt` when status moves to/from 'تم التسليم'. 
- **Constraints**: Debt logic only fires if `pricePerPiece > 0`. 

### 8.2.6 Class: Worker 
- **Purpose**: Represents factory floor personnel for tracking human resources, attendance, and payroll. 
- **Attributes**: 
  - `name` (String): Employee name. Required. 
  - `role` (String): Job title. Required. 
  - `salary` (Number): Monthly base pay. Required. 
  - `startDate` (String): Employment start date. 
  - `presentDays` (Number): Counter for attendance. Default: 0. 
  - `absentDays` (Number): Counter for missed work. Default: 0. 
  - `deductions` (Number): Financial penalties for the month. Default: 0. 
  - `notes` (String): Performance or attendance remarks. 
- **Operations**: 
  - `calculateNetSalary()`: Returns Number. Logic: `this.salary - this.deductions`. 
- **Constraints**: `deductions` cannot exceed the base salary. 

## 9. Operational Scenarios 
This section describes end-to-end transactions from the user's perspective, illustrating how various actors interact with the system logic to achieve specific goals. 

### 9.1 Scenario 1: Retail Purchase with AI Virtual Try-On 
**Actor**: BrandCustomer | **Goal**: Visualize a product on a personal photo and complete a retail purchase. 

| Step | Action | System Response | Logic Reference |
| :--- | :--- | :--- | :--- |
| 1 | User browses catalog and selects "Linen Blouse". | System fetches product details and current stock level. | `GET /products/:id` |
| 2 | User uploads a personal photo and clicks "Try It On". | System sends image data to yisol/IDM-VTON via Hugging Face API. | `POST /vto` |
| 3 | User views the synthesized image and adds item to cart. | System validates `hasStock()` and updates the Cart Context. | `Product.hasStock()` |
| 4 | User enters delivery details (e.g., Cairo) and checks out. | System retrieves shipping fee from Settings and calculates `totalAmount`. | `Order.pre('save')` |
| 5 | User confirms the order. | System atomically decrements stock, sends confirmation email, and logs to n8n. | `POST /orders` |

### 9.2 Scenario 2: B2B Wholesale Production & Debt Lifecycle 
**Actors**: FactoryClient, Owner | **Goal**: Submit a bulk manufacturing request and reconcile financial debt. 

| Step | Action (Agent) | System Response | Logic Reference |
| :--- | :--- | :--- | :--- |
| 1 | Client: Submits design specs and quantity per size (e.g., M: 500). | System creates a new order with status `في انتظار التسعير`. | `POST /wholesale-orders` |
| 2 | Owner: Reviews design and sets `pricePerPiece` (e.g., 150 EGP). | System calculates `totalPrice` and moves status to `قيد الانتظار`. | WholesaleOrder logic |
| 3 | Owner: Updates status as production progresses (Cutting > Sewing). | System updates the B2B portal in real-time for the client to track. | `PUT /wholesale-orders/:id` |
| 4 | Owner: Marks the order as `تم التسليم` (Delivered). | System checks `isDebtAdded` and atomically increments `FactoryClient.totalDebt`. | Debt Toggle Logic |
| 5 | Client: Pays a portion of the debt via offline bank transfer. | System (Owner) records `paidAmount` and updates the balance ledger. | `POST /factory-clients/:id/payment` |

### 9.3 Scenario 3: Secure Workforce Payroll Management 
**Actor**: Owner | **Goal**: Manage factory floor personnel and calculate net monthly salaries. 

| Step | Action | System Response | Logic Reference |
| :--- | :--- | :--- | :--- |
| 1 | Owner registers a new "Tailor" with a base salary of 8000 EGP. | System creates a Worker document with zeroed counters. | `POST /workers` |
| 2 | Owner logs daily attendance and absent days. | System updates `presentDays` and `absentDays` metrics. | Worker Schema |
| 3 | Owner enters a deduction (e.g., 200 EGP) for a specific worker. | System validates that `deductions <= salary`. | Worker Constraints |
| 4 | Owner views the payroll summary. | System dynamically computes `salary - deductions` for the final report. | `calculateNetSalary()` |

### 9.4 Scenario 4: Critical Account Recovery 
**Actor**: Any Registered User | **Goal**: Securely regain access to the platform after forgetting credentials. 

| Step | Action | System Response | Logic Reference |
| :--- | :--- | :--- | :--- |
| 1 | User clicks "Forgot Password" and enters their registered email. | System generates a SHA-256 hashed token and sets a 10-minute expiry. | `createPasswordResetToken` |
| 2 | System sends an automated HTML email via Nodemailer. | User receives a secure, short-lived reset link. | Nodemailer / SMTP |
| 3 | User clicks the link and enters a new password. | System validates the token hash and timestamp. | `PATCH /users/reset-password/:token` |
| 4 | User submits the new password. | System re-hashes the new password and clears the recovery fields. | `User.pre('save')` |

### 9.5 Scenario 5: Strategic Inventory Management & Updates 
**Actor**: Owner / Admin | **Goal**: Expand the retail collection and manage stock alerts. 

| Step | Action | System Response | Logic Reference |
| :--- | :--- | :--- | :--- |
| 1 | Owner adds a new product with HEX colors and multi-image array. | System validates required fields and persists to products collection. | `POST /products` |
| 2 | Owner updates the price or stock level for an existing item. | System updates the document and recalculates `inStock` boolean. | `PUT /products/:id` |
| 3 | Owner views the dashboard and identifies items with stock < 5. | System visually flags critical stock risks for replenishment. | Inventory Risk Logic |
| 4 | Owner deletes an obsolete product from the catalog. | System removes the document and ensures it's no longer browsable. | `DELETE /products/:id` |

### 9.6 Scenario 6: B2B Client Self-Service & Financial Transparency 
**Actor**: FactoryClient | **Goal**: Securely monitor specific manufacturing progress and debt balance. 

| Step | Action | System Response | Logic Reference |
| :--- | :--- | :--- | :--- |
| 1 | Client logs in via the private B2B portal using unique credentials. | System validates FactoryClient model and grants session access. | `POST /factory-clients/login` |
| 2 | Client accesses their dashboard. | System filters wholesaleorders to return only data matching their `clientId`. | `GET /wholesale-orders?clientId=...` |
| 3 | Client views their live ledger (`totalDebt` vs `paidAmount`). | System computes the balance due in real-time. | `getBalance()` Logic |
| 4 | Client tracks the production stage (e.g., `جاري الخياطة`) of their design. | System provides real-time manufacturing status updates. | WholesaleOrder Status |

*(Note: Scenarios 7-14 follow the exact same tabular structure for consistency).*

## 10. Preliminary Schedule Adjusted 
This section outlines the project plan, highlighting major milestones, interdependencies, and resource requirements for the development of the Fluffy Platform. 

### 10.1 Project (Development Timeline) 
The development lifecycle spans a 4-month period, structured into distinct phases from initial analysis to final deployment. 

| Phase | Task Name | Duration | Status |
| :--- | :--- | :--- | :--- |
| Analysis & Design | Requirements Gathering (SRS) | 5 Days | Completed |
| Analysis & Design | OO Analysis & UML Design | 3 Days | Completed |
| Analysis & Design | Database Schema Design | 4 Days | Completed |
| Backend & DB | Core API (Auth/Products/Orders) | 7 Days | Completed |
| Backend & DB | Wholesale & Debt Logic | 2 Days | Completed |
| Frontend | B2C Storefront Development | 3 Days | Completed |
| Frontend | B2B & Admin Portals | 3 Days | Completed |
| AI & Automation | AI Virtual Try-On Integration | 1 Days | Completed |
| AI & Automation | n8n Webhooks & SMTP Config | 1 Days | Completed |
| Testing & Launch | System Integration Testing | 5 Days | Completed |
| Testing & Launch | Deployment & Documentation | 10 Days | Completed |

### 10.2 Major Tasks and Interdependencies 
1. **Task: Core API -> Frontend (F1)**: The Retail Storefront depends on the product and order endpoints to display live data. 
2. **Task: Wholesale Logic -> B2B Portal (F2)**: The Factory Portal requires the status transition and debt increment logic to be finalized in the backend. 
3. **Task: AI VTO -> Product Detail**: The Virtual Try-On feature requires the `@gradio/client` and valid product images to synthesize previews. 
4. **Task: Settings Config -> Order Calculation**: Dynamic shipping fees must be set before the order pre-save hook can calculate `totalAmount`. 

## 11. Preliminary Budget Adjusted 
This section provides an initial itemized budget for the Fluffy Platform, categorizing expenses based on human resources, infrastructure, and third-party service integration. 

| Cost Factor | Description | Estimated Amount (EGP) |
| :--- | :--- | :--- |
| Human Resources | 1 Full-Stack Dev (MERN), 1 UI/UX Designer, 1 QA Tester (4 Months). | 6,000 |
| Cloud Infrastructure | Hosting (Vercel/Heroku), MongoDB Atlas (Tiered storage for images). | 12,000 |
| AI Inference Fees | Hugging Face Pro/ZeroGPU credits for Virtual Try-On synthesis. | 8,500 |
| SaaS & Automation | n8n cloud subscription and Professional SMTP (SendGrid/Gmail). | 3,000 |
| Assets & Security | Domain registration (.com / .com.eg) and SSL Certificates. | 2,500 |
| Contingency Fund | Reserved for unforeseen technical issues or scaling needs (10%). | 5,000 |
| **Total Estimated Budget** | | **37,000 EGP** |

## 12. Appendices 

### 12.1 Definitions, Acronyms, Abbreviations 

| Term | Definition |
| :--- | :--- |
| **JWT** | JSON Web Token - A stateless security standard for transmitting role-based claims between the client and server. |
| **VTO** | Virtual Try-On - A Generative AI process using latent diffusion (IDM-VTON) to visualize garments on human images. |
| **Atomic `$inc`** | A thread-safe MongoDB operation that ensures data consistency during concurrent stock or debt updates. |
| **RBAC** | Role-Based Access Control - The mechanism used to restrict system access to Owner, Admin, or Customer roles. |
| **n8n** | A workflow automation tool used by the platform to synchronize order data with external Google Sheets. |
| **B2B / B2C** | Business-to-Business (Factory Wholesale) and Business-to-Consumer (Brand Retail) commercial models. |
| **Pre-save Hook** | Mongoose middleware used in orderSchema to calculate `totalAmount` automatically before persistence. |
| **MERN Stack** | The core development stack: MongoDB (DB), Express (API), React (UI), and Node.js (Runtime). |
| **ERP** | Enterprise Resource Planning - The administrative module managing workers, attendance, and payroll. |
| **ZeroGPU** | A specialized Hugging Face hosting environment used for executing the GPU-intensive VTO AI model. |
