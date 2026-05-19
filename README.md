## 4.1.2 CLI (Command Line Interface) 
The platform utilizes a CLI for backend administration, environment configuration, and server-side diagnostics. 

| Command | Arguments | Example Invocation | Description |
| :--- | :--- | :--- | :--- |
| `npm run dev` | N/A | `npm run dev` | Initializes the Vite frontend and Express backend in development mode. |
| `node server.js` | N/A | `node server.js` | Launches the production-ready Node.js server and initializes the MongoDB connection. |
| `npm install` | Package Name | `npm i @gradio/client` | Synchronizes local `node_modules` with the `package.json` manifest. |

## 4.1.3 API (Application Programming Interface) 
The system exposes a RESTful JSON API via the `express.Router` middleware. All public and protected endpoints are described below. 

### 4.1.3.1 Product APIs 
* **GET `/products`**
  * **Args:** `category` (Optional Query). 
  * **Return:** List of products with `inStock` boolean computed server-side. 
  * **Interaction:** Feeds the Retail Shop grid. 
* **POST `/products`**
  * **Args:** `name`, `price`, `description`, `images`, `category`, `sizes`, `colors`, `stock`. 
  * **Return:** Newly created Product object. 
  * **Interaction:** Used by Owner Dashboard to expand the catalog. 

### 4.1.3.2 Retail & Wholesale Order APIs 
* **POST `/orders`**
  * **Args:** `customerName`, `email`, `phone`, `address`, `governorate`, `shippingFee`, `items[]`. 
  * **Return:** `orderId` and confirmation status. 
  * **Interaction:** Triggers FR04 (Stock Sync), FR09 (n8n Webhook), and FR13 (Email Notification). 
* **PUT `/wholesale-orders/:id`**
  * **Args:** `status`, `pricePerPiece`, `quantityPerSize`. 
  * **Return:** Updated Wholesale Order. 
  * **Interaction:** Triggers FR07 (Debt Logic) when status is changed to 'تم التسليم'.

### 4.1.3.3 Authentication & AI APIs 
* **POST `/users/login`**
  * **Args:** `email`, `password`. 
  * **Return:** token (JWT) and user profile. 
  * **Interaction:** Grants session access via JWT stored in LocalStorage. 
* **POST `/vto`**
  * **Args:** `humanImage` (URL/Blob), `productImage` (URL/Blob), `category`. 
  * **Return:** `resultImage` (Hugging Face URL). 
  * **Interaction:** Proxies data to the yisol/IDM-VTON model via `@gradio/client`. 

## 4.1.4 Diagnostics 
Diagnostic information is gathered and exposed through the following channels: 
* **Server Console Logs:** Real-time logging of MongoDB connectivity (`MongoDB connected successfully`), incoming HTTP requests, and Nodemailer SMTP transaction results. 
* **Standardized Error Responses:** The system utilizes a global error-handling middleware that returns JSON objects with `status: "error"` and a descriptive message (e.g., *ZeroGPU quotas exceeded* for AI failures). 
* **n8n Webhook Auditing:** Every retail order is echoed to a third-party webhook URL for external logging in Google Sheets, providing a diagnostic trail outside of the primary database. 

## 4.2 Hardware Interfaces 
* **Input/Output Devices:** Standard support for keyboard, mouse, and touchscreens. High-resolution displays are required for optimal garment visualization. 
* **Storage Interface:** The system interfaces with physical SSD storage on cloud servers via the MongoDB Atlas data stream protocol. 
* **AI Compute Interface:** Offloads heavy GPU processing to external Hugging Face Hardware (Inference Endpoints) via the Gradio HTTP protocol. 
 
## 4.3 Communications Interfaces 
* **HTTPS/TLS:** All data in transit between the React frontend and Node.js backend is secured via HTTPS on port 3000 (Local) or 443 (Production). 
* **SMTP Protocol:** The system interfaces with Gmail SMTP servers (Port 587/465) using the Nodemailer library for automated HTML dispatch. 
* **Webhooks (n8n):** Outgoing data streams use asynchronous HTTP POST requests to communicate with the n8n automation engine. 
* **External API Streams:** Secure WebSocket and HTTP streams connect the backend to Hugging Face Inference clusters for AI image processing. 

## 4.4 Software Interfaces 
The system integrates with the following software products and libraries: 
* **Mongoose ORM:** Interfaces with MongoDB for schema-driven data persistence and aggregation. 
* **JWT Service:** Uses the `jsonwebtoken` library to generate and verify stateless authorization tokens. 
* **Gradio Client:** Software interface for piping image data to the IDM-VTON generative AI model. 
* **Bcryptjs:** Cryptographic interface for secure password hashing before persistence. 
* **Nodemailer:** Software library for interfacing with Mail Transfer Agents (MTA) to generate dynamic HTML emails. 

## 5. Performance Requirements 

### 5.2.1 Client-Side (Frontend) 
* **Browser RAM:** The application shall maintain memory usage below 150 MB during standard browsing operations.  
* **Bundle Size:** Static assets shall be optimized using Vite to ensure the initial transferred bundle size remains under 2 MB (Gzipped).  
* **Image Caching:** Browser-level caching mechanisms shall be applied to product images to reduce redundant network transfers and improve loading performance.  

### 5.2.2 Server-Side (Backend) 
* **Node.js Process Usage:** The backend service shall consume less than 256 MB RAM while idle.  
* **Concurrency Handling:** The system shall support up to 100 concurrent requests per second using Node.js non-blocking I/O architecture.  
* **Database Storage:** MongoDB Atlas tiered storage shall be utilized to support scalable storage for order logs and manufacturing design images.  
	 
### 5.3 Reliability and Throughput 
* **Atomic Consistency:** Real-time inventory updates shall use MongoDB atomic `$inc` operations to prevent race conditions during high-traffic transactions.  
* **AI Service Fallback:** If Hugging Face ZeroGPU quota limits are exceeded, the system shall degrade gracefully by returning a mocked response within 2 seconds instead of timing out.  
* **API Availability:** Core commerce APIs shall target an uptime availability of 99.5%.  
	 
### 5.4 Capacity Constraints 

| Metric | Limit | Reason |
| :--- | :--- | :--- |
| Upload Size | 10MB per Image | Prevents server memory overflow during design uploads. |
| VTO Quota | Varies by HF Token | Limited by external API ZeroGPU credits. |
| Inventory Alert | Threshold = 5 | Triggers low-stock visual flags in real-time. |

## 6. Design Constraints 
This section defines the technical and operational limitations that influence the architecture and implementation of the Fluffy Platform. 
	 
### 6.1 Standards Compliance 
* **Security Protocols:** The system shall enforce industry-standard security mechanisms, including Bcrypt for secure one-way password hashing and JWT (JSON Web Tokens) for stateless authentication and session management.  
* **API Architecture:** Backend services shall follow RESTful API principles using standard HTTP methods and JSON-based communication formats.  
* **Coding Standards:** The frontend application shall be developed using TypeScript to ensure strict type safety, while the backend shall use ECMAScript Modules (ESM) to support modern modular development practices.  
* **UI Frameworks:** User interface components shall be restricted to Radix UI primitives and Shadcn/UI to maintain accessibility, consistency, and reusable design patterns.  
	 
### 6.2 Hardware Limitations 
* **Server Resources:** The backend service shall operate within a maximum idle memory consumption threshold of 256 MB RAM to ensure deployment compatibility with entry-level cloud hosting environments.  
* **Client Accessibility:** The web application shall remain responsive and performant on mobile devices with limited processing capabilities and varying screen resolutions.  
* **AI Compute Offloading:** Due to the high GPU computational requirements of AI-based image synthesis, the Virtual Try-On (VTO) feature shall not execute on local server hardware and must instead rely on external inference infrastructure provided by Hugging Face.  
 	 
### 6.3 Operational Constraints 
* **Database Constraints:** Data persistence shall be exclusively managed using MongoDB with the Mongoose ORM, enforcing a schema-driven structure within a NoSQL database environment.  
* **Third-Party Dependencies:** Several core platform functionalities depend on the availability of external services, including:  
  * **Hugging Face:** Required for AI-powered Virtual Try-On image synthesis.  
  * **Gmail SMTP:** Required for automated email notifications and message dispatching.  
  * **n8n:** Required for automated external data logging into Google Sheets.  
* **Language Synchronization:** The platform shall maintain consistency between the English-based backend implementation and the Arabic-localized frontend user interface.
