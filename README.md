# 📋 Fluffy System Requirements Specification (SRS)

هذا الملف يحتوي على التوثيق الشامل لكافة متطلبات نظام **Fluffy**، مصاغة وفقاً للقواعد الصارمة لكتاب "Software Requirement Patterns" مع الالتزام التام بالتصنيفات المطلوبة.

---

## 1️⃣ Functional Requirements (المتطلبات الوظيفية)

### Pattern Type: Inter-system Interface
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-01 |
| **Requirement name** | n8n Webhook Interface |
| **Requirement description** | There shall be a clearly defined interface (called i-n8n) between the Fluffy Backend and the n8n Orchestration System. Purposes: 1. Transmit new order payloads. 2. Transmit order cancellations. Interactions across this interface can be initiated by the Fluffy Backend only. This interface's definition is the responsibility of and owned by the Operations Manager. |
| **Source** | Operations Manager |
| **Owner** | Product Owner |
| **Author** | Business Analyst |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Logistics |
| **Stakeholders** | Logistics Team |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Order succeeds if n8n down. Logs failure. |
| **Related requirements** | NFR-22, FR-17 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Implemented as a fire-and-forget asynchronous call to ensure order creation is not blocked by downstream workflow issues. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inter-system Interface
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-02 |
| **Requirement name** | AI Virtual Try-On Interface |
| **Requirement description** | There shall be a clearly defined interface (called i-ai-vto) between the Fluffy Backend and the HuggingFace Gradio API. Purposes: 1. Send user images to get AI composite try-on results. Interactions across this interface can be initiated by the Fluffy Backend only. This interface's definition is the responsibility of and owned by the Tech Lead. |
| **Source** | Product Owner |
| **Owner** | Tech Lead |
| **Author** | Architect |
| **Type of requirement** | Functional |
| **Priority** | Critical |
| **Business area** | AI Experience |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-26, NFR-41 |
| **Acceptance criteria** | Fallback within 2s on timeout. |
| **Related requirements** | FR-44 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Core AI feature. Uses Hugging Face Gradio client. Must not block the UI indefinitely and falls back gracefully. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inter-system Interface
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-03 |
| **Requirement name** | Email Service |
| **Requirement description** | There shall be a clearly defined interface (called i-email-smtp) between the Fluffy Backend and the Gmail SMTP Service. Purposes: 1. Send transactional emails asynchronously. Interactions across this interface can be initiated by the Fluffy Backend only. This interface's definition is the responsibility of and owned by Customer Support. |
| **Source** | Customer Support |
| **Owner** | Product Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Notifications |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-25 |
| **Acceptance criteria** | Email non-blocking. |
| **Related requirements** | FR-54, FR-41 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Uses Nodemailer for non-blocking email dispatch to avoid delaying HTTP responses. Credentials securely stored in .env. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-04 |
| **Requirement name** | Customer Order Total Amount |
| **Requirement description** | Customer Order Total Amount shall be calculated as follows: totalAmount = sum(productPrice * qty) + shippingFee where productPrice is the snapshotted price from the DB; qty is the requested quantity; shippingFee is the delivery cost determined by FR-10. This calculation shall be performed server-side only. |
| **Source** | Finance |
| **Owner** | Finance Manager |
| **Author** | Architect |
| **Type of requirement** | Functional |
| **Priority** | Critical |
| **Business area** | Billing |
| **Stakeholders** | Finance |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | DB prices used. Total snapshotted. |
| **Related requirements** | NFR-22, FR-09 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Calculated inside the Order schema pre-save hook to ensure financial integrity and prevent client-side tampering. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-05 |
| **Requirement name** | Net Worker Salary |
| **Requirement description** | Net Worker Salary shall be calculated as follows: netSalary = max(0, salary - deductions) where salary is the worker's base monthly salary; deductions is the total deductions for the current month. |
| **Source** | HR |
| **Owner** | Atelier Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Payroll |
| **Stakeholders** | Workers, Owner |
| **Associated non functional requirements** | NFR-15 |
| **Acceptance criteria** | Displays 0 if deductions > salary. |
| **Related requirements** | FR-39 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Calculated dynamically on the frontend to prevent storing redundant derived data. Floor limits to zero. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-06 |
| **Requirement name** | Wholesale Order Total Price |
| **Requirement description** | Wholesale Order Total Price shall be calculated as follows: totalPrice = pricePerPiece * totalQuantity where pricePerPiece is the price per unit set by the Owner/Admin; totalQuantity is the sum of quantities across all sizes. |
| **Source** | B2B Sales |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B Billing |
| **Stakeholders** | Factory Clients |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Computed when admin sets price. |
| **Related requirements** | FR-62, FR-59 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Computed securely on the backend when the admin or owner sets the price per piece. Drives the B2B debt pipeline. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-07 |
| **Requirement name** | Wholesale Order Total Quantity |
| **Requirement description** | Wholesale Order Total Quantity shall be calculated as follows: totalQuantity = sum(quantityPerSize) where quantityPerSize is the quantity specified for each individual size. |
| **Source** | B2B Sales |
| **Owner** | Operations |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Production |
| **Stakeholders** | Factory Clients |
| **Associated non functional requirements** | NFR-14 |
| **Acceptance criteria** | Validated server side. |
| **Related requirements** | FR-06 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Re-calculated from the size breakdown object to prevent human data-entry errors. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-08 |
| **Requirement name** | Cart Subtotal |
| **Requirement description** | Cart Subtotal shall be calculated as follows: subtotal = sum(price * quantity) where price is the unit price of the cart item; quantity is the selected quantity of the cart item. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Cart |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Display updates instantly. |
| **Related requirements** | FR-09, FR-49 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Calculated purely for immediate user feedback. Real authoritative calculation occurs entirely on the backend. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Fee/Tax
**Group:** Chapter 12: Commercial
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-09 |
| **Requirement name** | Shipping Fee |
| **Requirement description** | The system shall calculate a Shipping Fee as a fixed amount on each customer retail order. It is paid by the Customer to the Atelier at checkout time. The fee rate shall be determined by the Governorate selected by the customer. The system is responsible for calculating the fee and adding it to the order total. |
| **Source** | Finance |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Logistics |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-16 |
| **Acceptance criteria** | Settings.rates used at checkout. |
| **Related requirements** | FR-10 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Stored separately from item totals in the database for accurate reporting and financial division. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-10 |
| **Requirement name** | Shipping Fee Lookup |
| **Requirement description** | Order Shipping Fee shall be calculated as follows: shippingFee = ShippingRates[governorate] ?? 0 where ShippingRates is the rates object from the Settings document; governorate is the string selected by the customer. |
| **Source** | Finance |
| **Owner** | Admin |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Checkout |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Unknown gov defaults to 0. |
| **Related requirements** | FR-25, FR-09 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Fetches dynamic rates from the Settings collection and defaults to zero if missing to prevent checkout blocks. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-11 |
| **Requirement name** | Factory Client Remaining Debt |
| **Requirement description** | Remaining Debt shall be calculated as follows: remainingDebt = totalDebt - paidAmount where totalDebt is the sum of all delivered wholesale order totalPrices; paidAmount is the sum of all cash payments received. |
| **Source** | Finance |
| **Owner** | Finance |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B |
| **Stakeholders** | Factory Clients |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Negative is credit balance. |
| **Related requirements** | FR-34, FR-62 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Dynamically calculated on the frontend by subtracting paid amount from total debt to avoid redundant DB columns. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-12 |
| **Requirement name** | Average Product Rating |
| **Requirement description** | Average Product Rating shall be calculated as follows: avgRating = round(sum(rating) / reviewsLength) where rating is the integer 1-5 from each review; reviewsLength is the total number of reviews (returns 0 if reviewsLength = 0). |
| **Source** | Marketing |
| **Owner** | PO |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Catalogue |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-08 |
| **Acceptance criteria** | 0 if no reviews. |
| **Related requirements** | NFR-18 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Averaged safely on the client side, guarding against empty review arrays and NaN exceptions. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-13 |
| **Requirement name** | Total Revenue |
| **Requirement description** | Total Revenue shall be calculated as follows: totalRevenue = sum(totalAmount) where totalAmount is the server-computed total stored at checkout time for each order. |
| **Source** | Finance |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Analytics |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Displayed in dashboard stats. |
| **Related requirements** | FR-33, FR-35 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Aggregated on the frontend for dashboard statistics to give a fast, real-time overview. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Calculation formula
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-14 |
| **Requirement name** | Total Workers Payroll |
| **Requirement description** | Total Payroll Liability shall be calculated as follows: totalPayroll = sum(salary) where salary is the base monthly salary for each registered worker. |
| **Source** | HR |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Payroll |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Uses base salaries. |
| **Related requirements** | FR-05, FR-37 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Aggregated on the dashboard to give the owner an immediate snapshot of financial payroll obligations. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-15 |
| **Requirement name** | Color Distribution Breakdown |
| **Requirement description** | There shall be a Color Distribution Breakdown inquiry that shows a visual, percentage-based breakdown of color trends across customer orders. Its purpose is to assist the owner in immediate manufacturing color-allocation choices. For each color, the inquiry shall show the following: Calculated percentage distribution. |
| **Source** | Marketing |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | Low |
| **Business area** | Analytics |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Renders exact calculated percentage distributions correctly on the dashboard view. |
| **Related requirements** | FR-38 |
| **Related documents** | src/components/dashboard/AIPredictionsTab.tsx |
| **comments** | Implemented as a client-side statistical aggregation on the dashboard to assist the owner in immediate manufacturing color-allocation choices. |
| **Version history** | 1.1 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-16 |
| **Requirement name** | Low-Stock Alerts Inquiry |
| **Requirement description** | There shall be a Low-Stock Alerts inquiry that shows a real-time list of inventory items flagged as 'At Risk'. Its purpose is to prevent stockouts by prompting the atelier owner to initiate new B2B manufacturing pipelines early. For each product, the inquiry shall show the following: Product details for items with stock > 0, stock <= 15, and soldCount > 0. The items to show shall be listed in ascending order of remaining stock sequence. |
| **Source** | Ops |
| **Owner** | Ops |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Inventory |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Instantly flags and isolates styles with stock levels between 1 and 15 inclusive. |
| **Related requirements** | NFR-19, FR-38 |
| **Related documents** | src/components/dashboard/AIPredictionsTab.tsx |
| **comments** | Operational dashboard lookup designed to prevent stockouts by prompting the atelier owner to initiate new B2B manufacturing pipelines early. |
| **Version history** | 1.1 |

<br>

### Pattern Type: System function
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-17 |
| **Requirement name** | Failed Webhook Error Logging |
| **Requirement description** | There shall be an interaction across interface i-n8n. The interaction shall be initiated by the Fluffy Backend. The interaction shall consist of capturing any error during asynchronous transmission and outputting a structured warning message containing the error message and timestamp to the server logs. |
| **Source** | DevOps |
| **Owner** | Tech Lead |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Audit |
| **Stakeholders** | Admins |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Emits standard error (console.error) to stdout without crashing or stopping the core order creation process. |
| **Related requirements** | FR-01 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Handled inside the catch block of order controllers to secure unblocked checkouts while keeping downstream workflow failures visible via system environment outputs. |
| **Version history** | 1.1 |

<br>

### Pattern Type: Chronicle
**Group:** Chapter 7: Data Entity
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-18 |
| **Requirement name** | Unhandled Error Chronicle |
| **Requirement description** | Every unhandled server exception (Global 500 error) shall automatically be recorded. For each, the following shall be recorded: Date and time, Stack trace, Request URL, Request method, and User ID (if authenticated). Each such event shall be treated as having a severity of Severe. |
| **Source** | Security |
| **Owner** | Tech Lead |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | Critical |
| **Business area** | Security |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | NFR-32 |
| **Acceptance criteria** | Generic 500 returned. Details logged. |
| **Related requirements** | NFR-32 |
| **Related documents** | server.js |
| **comments** | Catches global exceptions to prevent server crashes and avoid leaking stack traces to potential attackers. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-26 |
| **Requirement name** | Manual Monthly Payroll Adjustment |
| **Requirement description** | The system shall provide a function to update Worker monthly payroll counters, enabling the reset of presentDays, absentDays, and deductions to zero at the end of each billing month. |
| **Source** | HR |
| **Owner** | Owner |
| **Author** | Backend / Frontend |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Payroll |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-20 |
| **Acceptance criteria** | Owner can dynamically set active monthly fields back to 0 or adjust them per employee. |
| **Related requirements** | FR-20, FR-23 |
| **Related documents** | src/pages/OwnerDashboard.tsx, COMPLETE_BACKEND_CODE.js |
| **comments** | Enforced via individual UI counter adjustments allowing the owner total operational flexibility to reset or audit metrics per worker before issuing net payouts. |
| **Version history** | 1.1 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-19 |
| **Requirement name** | Product CRUD |
| **Requirement description** | The system shall provide functions to Create, Read, Update, and Delete Product records. The following operations shall be available: Create (POST), Read (GET), Update (PUT), Delete (DELETE). |
| **Source** | Ops |
| **Owner** | Admin |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Inventory |
| **Stakeholders** | Admins |
| **Associated non functional requirements** | NFR-19 |
| **Acceptance criteria** | Auth required. |
| **Related requirements** | FR-27 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Provides full CRUD backend endpoints protected by admin authorization to maintain product listings. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-20 |
| **Requirement name** | Worker CRUD |
| **Requirement description** | The system shall provide functions to Create, Read, Update, and Delete Worker records. The following operations shall be available: Create, Read, Update, Delete, Attendance update, Deductions update, Monthly reset. |
| **Source** | HR |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | HR |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-20 |
| **Acceptance criteria** | Auth required. Updates counters. |
| **Related requirements** | FR-23 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Provides backend endpoints for HR management, creating and associating worker profiles safely. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-21 |
| **Requirement name** | Factory Client CRUD |
| **Requirement description** | The system shall provide functions to Create, Read, Update, and Delete Factory Client records. The following operations shall be available: Create, Read, Record Payment, Update, Delete. |
| **Source** | Ops |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | NFR-21 |
| **Acceptance criteria** | Passwords hashed. Auth required. |
| **Related requirements** | FR-28 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Provides backend endpoints for managing B2B clients independently from retail customers. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-22 |
| **Requirement name** | Cancel Order & Restore |
| **Requirement description** | The system shall provide a function to update the status of a Customer Order to 'ملغي' and atomically restore Product stock quantities. |
| **Source** | Support |
| **Owner** | Ops |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Orders |
| **Stakeholders** | Support |
| **Associated non functional requirements** | NFR-22 |
| **Acceptance criteria** | Stock incremented correctly. |
| **Related requirements** | FR-17 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Atomically restores product stock quantities upon order cancellation to prevent phantom inventory losses. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-23 |
| **Requirement name** | Worker Attendance |
| **Requirement description** | The system shall provide a function to update Worker attendance records by incrementing or decrementing present/absent days. |
| **Source** | HR |
| **Owner** | Owner |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | HR |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-20 |
| **Acceptance criteria** | Minimum bounded to 0. |
| **Related requirements** | FR-26 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Quick UI controls for managing daily attendance and calculating penalties seamlessly. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-24 |
| **Requirement name** | Wholesale Status Transitions |
| **Requirement description** | The system shall provide a function to update the status of a Wholesale Order. Statuses shall advance forward only. |
| **Source** | B2B |
| **Owner** | Ops |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Production |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Transitions trigger debt increments. |
| **Related requirements** | FR-62, FR-59 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Enforces forward-only transitions and applies financial side effects when orders are delivered. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-25 |
| **Requirement name** | Global Shipping Config |
| **Requirement description** | The system shall provide a function to update System Settings records for global shipping fees. |
| **Source** | Finance |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Settings |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-16 |
| **Acceptance criteria** | Upserts Settings document. |
| **Related requirements** | FR-09 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Allows the owner to dynamically update shipping rates via the UI without a single line of code change. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-27 |
| **Requirement name** | Manual Stock Adjustment |
| **Requirement description** | The system shall provide a function to update Product stock by decrementing the value safely via a manual adjustment. |
| **Source** | Ops |
| **Owner** | Admin |
| **Author** | API |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Inventory |
| **Stakeholders** | Admins |
| **Associated non functional requirements** | NFR-19 |
| **Acceptance criteria** | Rejects negative final values. |
| **Related requirements** | FR-19 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Uses an atomic $inc decrement endpoint to safely adjust stock, preventing destructive overwriting by concurrent requests. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Transaction
**Group:** Chapter 7: Data Entity
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-28 |
| **Requirement name** | Record Cash Payment |
| **Requirement description** | There shall be a function to create a Factory Client Payment transaction for a Factory Client. Each Factory Client Payment shall contain the following information: amount, clientId. A Factory Client Payment is a financial ledger event. Each Factory Client Payment is uniquely identified by its transaction ID. A Factory Client Payment is deemed to have happened when the payment is recorded by the Owner. |
| **Source** | Finance |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Billing |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-23 |
| **Acceptance criteria** | Uses atomic $inc. |
| **Related requirements** | FR-11 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Admin-only operation to atomically register B2B cash payments to effectively manage ledger debts. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-29 |
| **Requirement name** | Cart Quantity Adjustment |
| **Requirement description** | The system shall provide a function to update Cart Item quantities in the client-side state, capped at maximum stock limits. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Cart |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | '+' disabled at max stock. |
| **Related requirements** | FR-08, FR-49 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Client-side limits to prevent adding more items than currently in stock. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-30 |
| **Requirement name** | Auto Stock Decrement |
| **Requirement description** | The system shall provide a function to update Product stock and soldCount automatically upon successful Customer Order creation. |
| **Source** | Ops |
| **Owner** | Sys Admin |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | Critical |
| **Business area** | Inventory |
| **Stakeholders** | System |
| **Associated non functional requirements** | NFR-22 |
| **Acceptance criteria** | Uses $inc. Validates stock >= qty. |
| **Related requirements** | FR-22 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Uses atomic $inc queries to update inventory accurately under concurrent load conditions. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-31 |
| **Requirement name** | Product Catalogue |
| **Requirement description** | There shall be a Product Catalogue inquiry that shows a filterable list view of products. Its purpose is to enable customers to browse and select products for purchase. For each product, the inquiry shall show the following: Product name, primary image, price, stock status, average rating. The items to show can be specified by entering any of the following selection criteria: Category. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Shop |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-33 |
| **Acceptance criteria** | Category filtering. |
| **Related requirements** | FR-19 |
| **Related documents** | src/pages/Shop.tsx |
| **comments** | Displays products with dynamic category filtering capabilities as the primary B2C portal. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-32 |
| **Requirement name** | Single Product Details |
| **Requirement description** | There shall be a Single Product Details inquiry that shows full product view with reviews and variant options. Its purpose is to provide complete product information to support the purchase decision. For each product, the inquiry shall show the following: All product fields, image gallery, colours, sizes, stock count, reviews, average rating. The items to show can be specified by entering any of the following selection criteria: Product ID from URL. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Shop |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-48 |
| **Acceptance criteria** | Renders all data correctly. |
| **Related requirements** | FR-43 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Displays comprehensive product data, variations, and reviews, allowing customers to build their orders. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-33 |
| **Requirement name** | Recent Orders |
| **Requirement description** | There shall be a Recent Orders inquiry that shows a dashboard view of orders. Its purpose is to allow Owner/Admin to monitor and manage incoming orders. For each order, the inquiry shall show the following: Order display ID, customer name, governorate, total amount, items summary, status, creation date. The items to show shall be listed in descending by createdAt sequence. |
| **Source** | Ops |
| **Owner** | Admin |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Orders |
| **Stakeholders** | Admins |
| **Associated non functional requirements** | NFR-22 |
| **Acceptance criteria** | Descending sort. |
| **Related requirements** | FR-35 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Displays chronological feed of customer orders for fulfillment and administrative monitoring. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-34 |
| **Requirement name** | Factory Wholesale History |
| **Requirement description** | There shall be a Factory Wholesale Orders History inquiry that shows a filtered view of factory orders locked to session client. Its purpose is to allow the Factory Client to review their manufacturing orders, prices, and statuses. For each order, the inquiry shall show the following: Model name, colours, quantity per size, totalQuantity, pricePerPiece, totalPrice, current status. The items to show can be specified by entering any of the following selection criteria: clientId (server-enforced). |
| **Source** | B2B |
| **Owner** | Owner |
| **Author** | Security |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B |
| **Stakeholders** | Factory Clients |
| **Associated non functional requirements** | NFR-38 |
| **Acceptance criteria** | Cannot bypass clientId filter. |
| **Related requirements** | NFR-47 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Server enforces that clients can only view their own wholesale orders by binding queries to their session context. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-35 |
| **Requirement name** | Dashboard Stats |
| **Requirement description** | There shall be a Dashboard Global Statistics inquiry that shows summary cards. Its purpose is to give the Owner an instant snapshot of business performance. For the system, the inquiry shall show the following: Total Revenue, Total Orders, Total Products, Total Factory Debt. |
| **Source** | Analytics |
| **Owner** | Owner |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | Admin |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Fast rendering. |
| **Related requirements** | FR-13 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Real-time calculation of key performance indicators for the business owner to monitor operations visually. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-36 |
| **Requirement name** | Factory Clients Roster |
| **Requirement description** | There shall be a Factory Clients Roster inquiry that shows an admin list of all clients with debt status. Its purpose is to monitor financial standing of all partner factories and identify outstanding debts. For each factory client, the inquiry shall show the following: Company name, owner name, total debt, paid amount, remaining debt, phone. The items to show can be specified by entering any of the following selection criteria: Live search by company name or owner name. |
| **Source** | Finance |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | B2B |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Searchable list. |
| **Related requirements** | FR-21, FR-11 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Lists all B2B clients and their current credit/debt balances for swift financial audits. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-37 |
| **Requirement name** | Workers Payroll List |
| **Requirement description** | There shall be a Workers Attendance & Payroll List inquiry that shows an admin view of all workers with live payroll data. Its purpose is to enable the Owner to review payroll obligations and attendance for the current month. For each worker, the inquiry shall show the following: Worker name, role, start date, base salary, deductions, net salary, present days, absent days, notes. The items to show shall be listed in descending by createdAt sequence. |
| **Source** | HR |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | HR |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-20 |
| **Acceptance criteria** | Populates User data. |
| **Related requirements** | FR-39 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Consolidates worker attendance and active payroll figures into a cohesive view. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Report
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-38 |
| **Requirement name** | AI Sales Predictions |
| **Requirement description** | There shall be a report that shows trends, forecasts, and at-risk items. The purpose of this report is to provide the Owner with data-driven insights for inventory replenishment and sales strategy. For the system, the report shall show the following: Top selling colours, top selling sizes, at-risk inventory items, projected monthly revenue. |
| **Source** | Analytics |
| **Owner** | Owner |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | Low |
| **Business area** | Strategy |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Aggregates all orders. |
| **Related requirements** | FR-16, FR-64, FR-15 |
| **Related documents** | src/components/dashboard/AIPredictionsTab.tsx |
| **comments** | Aggregates historical data to highlight trends and low-stock risks algorithmically. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Report
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-39 |
| **Requirement name** | Worker Payroll Report |
| **Requirement description** | There shall be a report that shows total payroll obligation and net sum. The purpose of this report is to enable the Owner to review and approve monthly salary disbursements. For each worker, the report shall show the following: Worker name, base salary, deductions, net salary, present days, absent days. Totals shall be shown for Total gross payroll, Total deductions, Total net payroll. |
| **Source** | Finance |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Payroll |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Totals displayed at bottom. |
| **Related requirements** | FR-37, FR-14 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Summary of net payouts required for the current billing cycle to facilitate simple disbursement logic. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Report
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-40 |
| **Requirement name** | Order Email Receipt |
| **Requirement description** | There shall be a report that shows an itemized bill. The purpose of this report is to provide the customer with a permanent, branded proof of purchase. For each order, the report shall show the following: Order display ID, product names, colours, sizes, quantities, unit prices, shipping fee, total amount. The report is intended to be run automatically after checkout. |
| **Source** | Comms |
| **Owner** | PO |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | UX |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Dispatched async. Translates colors. |
| **Related requirements** | NFR-09, FR-03 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Dispatches a formatted HTML receipt via SMTP asynchronously upon successful placement. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Report
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-41 |
| **Requirement name** | Password Reset Email |
| **Requirement description** | There shall be a report that shows a time-limited token link. The purpose of this report is to deliver the reset link securely and set correct user expectation about time-limited validity. For the user, the report shall show the following: Reset URL, validity warning. The report is intended to be run automatically upon request. |
| **Source** | Security |
| **Owner** | Security |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Security |
| **Stakeholders** | Users |
| **Associated non functional requirements** | NFR-42 |
| **Acceptance criteria** | 10 minute warning included. |
| **Related requirements** | FR-03 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Sends a secure, time-limited token link to the user's email while storing only its hash inside the database. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-42 |
| **Requirement name** | Checkout UI |
| **Requirement description** | The system shall provide a Checkout user interface. Its purpose is to collect delivery details and confirm the customer's purchase. It shall be used by Customers. It shall allow the user to enter customerName, phone, address, and governorate. It shall display the dynamic shipping fee update, order confirmation message, and trigger email receipt. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Cart |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-13 |
| **Acceptance criteria** | Validates all fields. |
| **Related requirements** | FR-52 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Collects user delivery information and validates fields before submission to the backend processing API. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-43 |
| **Requirement name** | Product Detail UI |
| **Requirement description** | The system shall provide a Product Detail user interface. Its purpose is to display full product information and enable variant selection before adding to cart. It shall be used by Customers. It shall allow the user to enter selected colour, selected size, and quantity. It shall display product images, price, description, stock status, average rating, and reviews. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Shop |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-24 |
| **Acceptance criteria** | Button disabled if out of stock. |
| **Related requirements** | FR-51 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Manages user selections for colour and size, and provides the Add to Cart action, enforcing stock limitations proactively. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-44 |
| **Requirement name** | VTO Modal |
| **Requirement description** | The system shall provide a Virtual Try-On Modal user interface. Its purpose is to allow customers to virtually try on garments using AI. It shall be used by Customers. It shall allow the user to enter a personal photo upload. It shall display a loading spinner during processing and the AI-generated composite image. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | AI |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-26 |
| **Acceptance criteria** | Shows spinner. Blocks double submits. |
| **Related requirements** | FR-02 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Interactive modal for AI try-on with loading states to prevent double submissions during extensive processing. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-45 |
| **Requirement name** | Owner Main Dashboard |
| **Requirement description** | The system shall provide an Owner Main Dashboard user interface. Its purpose is to enable comprehensive management of all atelier operations. It shall be used by Owner and Admin roles. It shall allow the user to enter tab navigation and modal triggers. It shall display four stat cards and six management tabs. |
| **Source** | Ops |
| **Owner** | Admin |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Admin |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-37 |
| **Acceptance criteria** | Protected by role. |
| **Related requirements** | FR-35 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Tabbed interface for managing the entire business, restricted to the owner by specialized frontend guards. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-46 |
| **Requirement name** | Login UI |
| **Requirement description** | The system shall provide a User Login user interface. Its purpose is to authenticate registered users and establish a session. It shall be used by All registered users. It shall allow the user to enter email address and password. It shall display a redirect to home on success or a generic error message on failure. |
| **Source** | Security |
| **Owner** | Sec Lead |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | Critical |
| **Business area** | Auth |
| **Stakeholders** | Users |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Stores JWT on success. |
| **Related requirements** | FR-52, FR-57 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Authentication interface ensuring secure JWT token handling and deliberately obscure error messaging. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-47 |
| **Requirement name** | Customer Registration UI |
| **Requirement description** | The system shall provide a Customer Registration user interface. Its purpose is to allow visitors to create a customer account. It shall be used by Unauthenticated visitors. It shall allow the user to enter full name, email, phone, and password. It shall display a success toast and redirect to login, or specific error messages. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Auth |
| **Stakeholders** | Visitors |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Success redirect to login. |
| **Related requirements** | FR-52, FR-54 |
| **Related documents** | src/pages/SignUp.tsx |
| **comments** | Registration form with real-time error handling and validation indicating issues explicitly to the client. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-48 |
| **Requirement name** | Password Reset UI |
| **Requirement description** | The system shall provide a Password Reset user interface. Its purpose is to allow a user to set a new password using a valid time-limited token. It shall be used by Users with a reset token. It shall allow the user to enter a new password. It shall display a success message and redirect, or an invalid token error. |
| **Source** | UX |
| **Owner** | Sec Lead |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Auth |
| **Stakeholders** | Users |
| **Associated non functional requirements** | NFR-40 |
| **Acceptance criteria** | Handles expired tokens. |
| **Related requirements** | FR-41 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Interface for securely establishing a new user password derived from validated emailed tokens. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-49 |
| **Requirement name** | Shopping Cart UI |
| **Requirement description** | The system shall provide a Shopping Cart user interface. Its purpose is to review and modify selected items before confirming purchase. It shall be used by Customers. It shall allow the user to enter quantity adjustments and item removals. It shall display item details, row subtotal, and cart grand total. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Cart |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Limits qty by stock. |
| **Related requirements** | FR-08, FR-29 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Client-side state management for pending purchases preventing requests that inherently violate stock ceilings. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-50 |
| **Requirement name** | Factory Portal UI |
| **Requirement description** | The system shall provide a Wholesale Factory Portal user interface. Its purpose is to enable Factory Clients to submit manufacturing orders and monitor their account. It shall be used by Factory Clients. It shall allow the user to enter order model name, colours, sizes, and design images. It shall display a debt summary and order history. |
| **Source** | B2B |
| **Owner** | Owner |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | NFR-38 |
| **Acceptance criteria** | Submits size breakdown. |
| **Related requirements** | FR-34 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Dedicated dashboard for B2B clients to submit specifications while isolating them from the core CMS systems. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-51 |
| **Requirement name** | Image Gallery UI |
| **Requirement description** | The system shall provide a Product Image Gallery user interface. Its purpose is to allow customers to explore multiple product images before purchasing. It shall be used by Customers. It shall allow the user to enter a click on a thumbnail image. It shall display a large active image display and a highlighted border on the selected thumbnail. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | Low |
| **Business area** | Shop |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-19 |
| **Acceptance criteria** | Thumbnail updates main. |
| **Related requirements** | FR-43 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Interactive gallery allowing users to examine product images in detail via fluid CSS and framer motion states. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-52 |
| **Requirement name** | Form Error Feedback |
| **Requirement description** | The system shall provide a Form Validation and Error Feedback user interface. Its purpose is to inform the user clearly when their input is rejected and why. It shall be used by All users of any form. It shall allow the user to enter invalid form data. It shall display an inline error message quoting the server's reason. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Core UI |
| **Stakeholders** | All |
| **Associated non functional requirements** | NFR-25 |
| **Acceptance criteria** | Uses axios interceptor/catch. |
| **Related requirements** | FR-53 |
| **Related documents** | src/pages/SignUp.tsx |
| **comments** | Surfaces backend validation and processing errors directly to the user to prevent dead feedback loops. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User interface
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-53 |
| **Requirement name** | Network Error UI |
| **Requirement description** | The system shall provide a Network Connectivity Error Feedback user interface. Its purpose is to distinguish server connectivity failures from validation errors. It shall be used by All users. It shall allow the user to enter a form submission during a network outage. It shall display an offline warning message. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Core UI |
| **Stakeholders** | All |
| **Associated non functional requirements** | NFR-32 |
| **Acceptance criteria** | Custom offline message. |
| **Related requirements** | FR-52 |
| **Related documents** | src/pages/SignUp.tsx |
| **comments** | Custom fallbacks indicating when the backend server is unreachable rather than failing silently. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User registration
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-54 |
| **Requirement name** | Customer Self-Registration |
| **Requirement description** | A person shall be able to self-register as a Customer, by submitting a form via SignUp.tsx. They shall be asked to enter the following personal information: Full name, email address (unique), password (minimum 6 characters). |
| **Source** | Auth |
| **Owner** | Sec Lead |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Identity |
| **Stakeholders** | Visitors |
| **Associated non functional requirements** | NFR-12 |
| **Acceptance criteria** | Bcrypt hash, ignores role body. |
| **Related requirements** | FR-57 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Automatically assigns customer role to prevent privilege escalation via injected payload elements. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User registration
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-55 |
| **Requirement name** | Worker Admin Registration |
| **Requirement description** | It shall be possible to register a person as a Worker, by submitting an administrative registration via Owner Dashboard. The following information shall be entered about them: Full name, role, salary, skills, start date. |
| **Source** | HR |
| **Owner** | Owner |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | HR |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-20 |
| **Acceptance criteria** | Creates User + Worker docs. |
| **Related requirements** | FR-20 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Allows owners to add staff accounts linked to their payroll records atomically in the database. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User registration
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-56 |
| **Requirement name** | Factory Client Registration |
| **Requirement description** | It shall be possible to register a person as a Factory Client, by administrative submission via Owner Dashboard. The following information shall be entered about them: Company name, owner name, phone, username, password. |
| **Source** | B2B |
| **Owner** | Owner |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-11 |
| **Acceptance criteria** | Unique username enforced. |
| **Related requirements** | FR-21 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Creates B2B accounts using unique usernames instead of emails, isolated strictly from normal client pools. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User authentication
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-57 |
| **Requirement name** | Customer & Staff Login |
| **Requirement description** | A Customer, Worker, or Admin shall be able to self-authenticate (log in) by providing their email and password. The user must authenticate before accessing protected routes. |
| **Source** | Security |
| **Owner** | Sec Lead |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | Critical |
| **Business area** | Auth |
| **Stakeholders** | All |
| **Associated non functional requirements** | NFR-44 |
| **Acceptance criteria** | bcrypt.compare used. |
| **Related requirements** | FR-54 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Issues secure JSON Web Tokens to establish sessions without demanding server-side state. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User authentication
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-58 |
| **Requirement name** | Factory Client Login |
| **Requirement description** | A Factory Client shall be able to self-authenticate (log in) by providing their Username and password. The user must authenticate before accessing the factory portal. |
| **Source** | B2B |
| **Owner** | Sec Lead |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | NFR-38 |
| **Acceptance criteria** | Separate localStorage key. |
| **Related requirements** | FR-56 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Dedicated authentication path for B2B partners keeping their cache states distinctly bound to their entities. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Approval
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-59 |
| **Requirement name** | Wholesale Pricing Approval |
| **Requirement description** | A Pricing of Wholesale Order and authorisation of production commencement must be approved by an Owner or Admin role only before the order advances to 'قيد الانتظار' and production commences. The order remains in 'في انتظار التسعير' until approved. |
| **Source** | Ops |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-39 |
| **Acceptance criteria** | Blocked if client attempts. |
| **Related requirements** | FR-24 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Prevents a wholesale order from progressing until the admin formally sets the price representing human-level approval. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Configurable authorization
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-60 |
| **Requirement name** | Role Assignment |
| **Requirement description** | An Owner shall be able to access the assignment of user roles to other users. The motivation for this requirement is to allow the owner to change the role field of any User (except their own Owner account) via the Owner Dashboard. |
| **Source** | Security |
| **Owner** | Owner |
| **Author** | API |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | Admin |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Owner only. Admin rejected. |
| **Related requirements** | NFR-37, FR-65 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Only the highest level owner can modify system roles preserving escalation control per security best practices. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data archiving
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-61 |
| **Requirement name** | Cancelled Orders Archiving |
| **Requirement description** | Customer Orders with status='ملغي' that were cancelled more than 90 days ago shall be moved from the active orders collection to the archive orders collection every month. The purpose of this is to free processing capacity. |
| **Source** | DBA |
| **Owner** | Admin |
| **Author** | DBA |
| **Type of requirement** | Functional |
| **Priority** | Low |
| **Business area** | Data |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | NFR-43 |
| **Acceptance criteria** | Monthly cron job. |
| **Related requirements** | NFR-31 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Moves legacy or abandoned orders out of the active operational collections to free processing capacity. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Fee/Tax
**Group:** Chapter 12: Commercial
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-62 |
| **Requirement name** | Factory Invoicing |
| **Requirement description** | The system shall calculate a Factory Invoice fee as a cumulative amount on Wholesale Order status transitions to 'تم التسليم'. It is paid by Factory Client to Atelier upon delivery. The fee rate shall be determined by the order's totalPrice. The system is responsible for incrementing FactoryClient.totalDebt by order.totalPrice using $inc. |
| **Source** | Finance |
| **Owner** | Finance |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | B2B Billing |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-47 |
| **Acceptance criteria** | Atomic $inc operation. |
| **Related requirements** | FR-11 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Updates the financial ledger when bulk goods are marked as delivered representing a cumulative invoice model. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data maintenance
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-63 |
| **Requirement name** | Edit Pending B2B Order |
| **Requirement description** | The system shall provide a function to update Wholesale Order records (colors, quantityPerSize, designImages, productionNotes). This operation shall be available only when the order status is 'في انتظار التسعير'. |
| **Source** | B2B |
| **Owner** | Ops |
| **Author** | API |
| **Type of requirement** | Functional |
| **Priority** | Medium |
| **Business area** | B2B |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | NFR-39 |
| **Acceptance criteria** | Rejected if locked. |
| **Related requirements** | FR-50 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Allows factories to revise orders before pricing is formally locked creating a clean submission workflow grace period. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inquiry
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-64 |
| **Requirement name** | Revenue Forecast |
| **Requirement description** | There shall be a report that shows a visual revenue projection. The purpose of this report is to give the Owner a directional revenue estimate for the current month. For the system, the report shall show the following: Month-to-date revenue (actual), projected full-month revenue (heuristic). |
| **Source** | Analytics |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Functional |
| **Priority** | Low |
| **Business area** | Strategy |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Labeled 'AI Prediction'. |
| **Related requirements** | FR-13, FR-38 |
| **Related documents** | src/components/dashboard/AIPredictionsTab.tsx |
| **comments** | Project month-end revenue by extrapolating current sales metrics using a linear heuristic approach. |
| **Version history** | 1.0 |

<br>

### Pattern Type: User authorization
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-65 |
| **Requirement name** | Role-Based Access Control Overview |
| **Requirement description** | A user shall not access any protected endpoint unless their JWT contains an authorized role payload mapping to the required privileges. |
| **Source** | Security |
| **Owner** | Sec Lead |
| **Author** | Architect |
| **Type of requirement** | Functional |
| **Priority** | Critical |
| **Business area** | Auth |
| **Stakeholders** | All |
| **Associated non functional requirements** | NFR-37 |
| **Acceptance criteria** | Middleware checks roles. |
| **Related requirements** | FR-57 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | The structural architecture protecting endpoints using JWT role payloads governing all specific authorization rules. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Inter-system Interaction
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | FR-66 |
| **Requirement name** | AI VTO Fallback Handler |
| **Requirement description** | There shall be an interaction across interface i-ai-vto. The interaction shall be initiated by the Fluffy Backend. The interaction shall consist of handling quota exceptions or timeouts by returning a mock image response. |
| **Source** | UX |
| **Owner** | Tech Lead |
| **Author** | Backend |
| **Type of requirement** | Functional |
| **Priority** | High |
| **Business area** | AI |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Returns mock image on quota error. |
| **Related requirements** | FR-02, NFR-26 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Prevents the UI from breaking or showing an endless spinner when external free-tier AI quotas are exhausted. |
| **Version history** | 1.1 |

---

## 2️⃣ Non-Functional Requirements (المتطلبات غير الوظيفية)

### Pattern Type: Technology
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-01 |
| **Requirement name** | Backend Server |
| **Requirement description** | Node.js with the Express.js framework shall be used for the server-side application runtime and HTTP routing. Node.js (>= 18) shall be supported. The motivation is to enable high concurrency for API requests without blocking I/O. |
| **Source** | Tech |
| **Owner** | Tech Lead |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | Backend |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | NFR-27 |
| **Acceptance criteria** | Non-blocking execution. |
| **Related requirements** | NFR-27 |
| **Related documents** | server.js |
| **comments** | Enables high concurrency for API requests without blocking I/O serving as the core architecture. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Technology
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-02 |
| **Requirement name** | Frontend Framework |
| **Requirement description** | React.js (SPA), Tailwind CSS, Framer Motion shall be used for all browser-rendered user interfaces. The motivation is to power the responsive, animated user interfaces through deep component reuse. |
| **Source** | Tech |
| **Owner** | Frontend Lead |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | UI |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-35 |
| **Acceptance criteria** | SPA build succeeds. |
| **Related requirements** | FR-51 |
| **Related documents** | src/pages/Index.tsx |
| **comments** | Powers the responsive, animated user interfaces through deep component reuse. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Technology
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-03 |
| **Requirement name** | Database |
| **Requirement description** | MongoDB and Mongoose ODM shall be used for sole persistent data store for all application entities. The motivation is to provide schema validation via Mongoose for all stored entities utilizing flexible NoSQL models. |
| **Source** | Data |
| **Owner** | DBA |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | DB |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | NFR-31 |
| **Acceptance criteria** | Schemas enforced. |
| **Related requirements** | NFR-19 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Provides schema validation via Mongoose for all stored entities utilizing flexible NoSQL models. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Technology
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-04 |
| **Requirement name** | CORS Policy |
| **Requirement description** | Express CORS middleware (Access-Control-Allow-Origin headers) shall be used for all Express routes in server.js. The motivation is to prevent unauthorized cross-origin browsers from calling protected API endpoints. |
| **Source** | Security |
| **Owner** | Sec Lead |
| **Author** | API |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Security |
| **Stakeholders** | Clients |
| **Associated non functional requirements** | NFR-01 |
| **Acceptance criteria** | Rejects unauthorized origins. |
| **Related requirements** | NFR-01 |
| **Related documents** | server.js |
| **comments** | Secures backend APIs against unauthorized cross-origin browser requests implicitly adding defense perimeters. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Technology
**Group:** Chapter 5: Fundamental
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-05 |
| **Requirement name** | Orchestration |
| **Requirement description** | n8n workflow automation platform shall be used for receiving order events and orchestrating logistics. The motivation is to centralise logistics orchestration outside the core application. |
| **Source** | Ops |
| **Owner** | Ops Manager |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Ops |
| **Stakeholders** | Logistics |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Payload matches n8n spec. |
| **Related requirements** | FR-01 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Delegates non-critical downstream logistics to external platforms rather than executing complex internal event routing. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data type
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-06 |
| **Requirement name** | Email Address Data Type |
| **Requirement description** | Email Addresses, which are used for unique user identification, shall be of the form String. Must be stored in all-lowercase with leading/trailing whitespace stripped. Must be unique within the User collection. |
| **Source** | Data |
| **Owner** | DBA |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | Auth |
| **Stakeholders** | Users |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Unique index matches format. |
| **Related requirements** | FR-57, FR-54, FR-03 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Standardises inputs to prevent duplicate registration issues resulting from erratic end-user casings. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data type
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-07 |
| **Requirement name** | Order Status Enum |
| **Requirement description** | Order Lifecycle Statuses, which are used for governing order lifecycle, shall be of the form String restricted to a defined enum. Values must be one of the enumerated lifecycle values. |
| **Source** | Orders |
| **Owner** | Ops |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | State |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | NFR-22 |
| **Acceptance criteria** | Rejects undefined statuses. |
| **Related requirements** | FR-24 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Strictly controls workflow states using pre-defined enum values bridging English and Arabic mappings safely. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data type
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-08 |
| **Requirement name** | Rating Score |
| **Requirement description** | Rating Scores, which are used for product reviews, shall be of the form Integer. Must be between 1 and 5 inclusive. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | Reviews |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-18 |
| **Acceptance criteria** | Rejects 0 or 6. |
| **Related requirements** | FR-12 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Ensures mathematical validity for product review aggregations bounded cleanly inside the model. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data type
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-09 |
| **Requirement name** | Colour Code |
| **Requirement description** | Colour Codes, which are used for product colors, shall be of the form CSS hex string. Must match the regex /^#[0-9A-Fa-f]{6}$/. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | Low |
| **Business area** | Catalogue |
| **Stakeholders** | Admins |
| **Associated non functional requirements** | NFR-24 |
| **Acceptance criteria** | Maps to Arabic name. |
| **Related requirements** | NFR-24 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Maintains consistent aesthetic mapping between UI elements and translated descriptions sent to clients. |
| **Version history** | 1.0 |

<br>

### Pattern Type: ID
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-10 |
| **Requirement name** | Order Display Reference |
| **Requirement description** | Each Customer Order shall have a unique ID that is in the form of the last 6 characters of ObjectId, uppercased allocated by slicing the MongoDB ObjectId. It shall be displayed to customers as a 6-character uppercase string. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | Orders |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-22 |
| **Acceptance criteria** | Shown on dashboard/receipts. |
| **Related requirements** | FR-33 |
| **Related documents** | src/pages/OwnerDashboard.tsx |
| **comments** | Extracts readable references from ObjectIDs for customer communication rendering exact keys unneeded for simple references. |
| **Version history** | 1.0 |

<br>

### Pattern Type: ID
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-11 |
| **Requirement name** | Factory Username |
| **Requirement description** | Each Factory Client shall have a unique ID that is in the form of an alphanumeric string allocated by the Owner manually. Each Factory Username shall be unique within the FactoryClient collection. |
| **Source** | B2B |
| **Owner** | Ops |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Auth |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Unique index enforced. |
| **Related requirements** | FR-58, FR-56 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Provides distinct organizational identifiers specifically for B2B authentication eliminating email dependency. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data structure
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-12 |
| **Requirement name** | Order Item Details |
| **Requirement description** | Order Item Details shall comprise the following items of information: productId, name, color, size, quantity, price snapshot. |
| **Source** | Data |
| **Owner** | DBA |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Orders |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | NFR-22 |
| **Acceptance criteria** | Contains productId and locked price. |
| **Related requirements** | FR-04 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Snapshots the state of purchased products to maintain exact historical records despite future inventory price evolutions. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data structure
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-13 |
| **Requirement name** | Shipping Address |
| **Requirement description** | Shipping Address Details shall comprise the following items of information: customerName, phone, address, governorate. |
| **Source** | Data |
| **Owner** | DBA |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Orders |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | NFR-22 |
| **Acceptance criteria** | All fields mandatory. |
| **Related requirements** | FR-42 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Embeds required destination fields within the order structure strictly coupling calculations to addresses. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data structure
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-14 |
| **Requirement name** | Wholesale Breakdown |
| **Requirement description** | Wholesale Order Breakdown shall comprise the following items of information: Object mapping size keys (S, M, L, XL, XXL) to non-negative integer quantities. |
| **Source** | B2B |
| **Owner** | Ops |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | B2B |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Integer validation. |
| **Related requirements** | FR-07 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Stores complex sizing configurations inside a flexible object mapping bypassing static array bindings. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data structure
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-15 |
| **Requirement name** | Worker Payroll Record |
| **Requirement description** | Worker Payroll Record shall comprise the following items of information: salary, deductions, presentDays, absentDays, notes. |
| **Source** | HR |
| **Owner** | Owner |
| **Author** | BA |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Payroll |
| **Stakeholders** | Admins |
| **Associated non functional requirements** | NFR-20 |
| **Acceptance criteria** | min 0 on fields. |
| **Related requirements** | FR-26 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Defines the numeric limits and defaults for calculating worker salaries resetting cleanly month by month. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data structure
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-16 |
| **Requirement name** | System Settings |
| **Requirement description** | System Settings Record shall comprise the following items of information: type (Category discriminator string), rates (Object with string keys and Number values). |
| **Source** | Config |
| **Owner** | DBA |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | Settings |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Upsertible. |
| **Related requirements** | FR-25 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Allows dynamic global key-value configuration overrides without code changes persisting inside unstructured docs. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data structure
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-17 |
| **Requirement name** | VTO Payload |
| **Requirement description** | VTO Prediction Request Payload shall comprise the following items of information: person_image, garment_image, category, guidance_scale, steps. |
| **Source** | AI |
| **Owner** | Tech Lead |
| **Author** | Backend |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | Integration |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Maps categories. |
| **Related requirements** | FR-02 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Formats outgoing data strictly to match third-party AI specifications circumventing type mismatches natively. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Living entity
**Group:** Chapter 7: Data Entity
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-19 |
| **Requirement name** | Product Entity |
| **Requirement description** | The system shall store the following information about a Product: name, price, description, images, category, colors, sizes, stock, soldCount, rating. A Product is the central entity in the retail operation. Each Product is uniquely identified by its ObjectId. |
| **Source** | Data |
| **Owner** | Ops |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | DB |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Mongoose bounds. |
| **Related requirements** | FR-19 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Defines the persistent catalogue entity and its required properties serving as the nexus for commercial structures. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Living entity
**Group:** Chapter 7: Data Entity
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-20 |
| **Requirement name** | Worker Entity |
| **Requirement description** | The system shall store the following information about a Worker: user reference, skills, salary, deductions, presentDays, absentDays, notes, startDate. A Worker is the persistent profile of a workshop employee. Each Worker is uniquely identified by its ObjectId. Parent entity is User. |
| **Source** | Data |
| **Owner** | HR |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | DB |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Reference integrity. |
| **Related requirements** | FR-20 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Enforces relationships between the authentication base and specific employment records scaling user patterns safely. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Living entity
**Group:** Chapter 7: Data Entity
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-21 |
| **Requirement name** | Factory Client Entity |
| **Requirement description** | The system shall store the following information about a Factory Client: companyName, ownerName, username, password, phone, totalDebt, paidAmount. A Factory Client is the persistent record for a B2B partner factory. Each Factory Client is uniquely identified by its Username. |
| **Source** | Data |
| **Owner** | Ops |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | DB |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Own auth fields. |
| **Related requirements** | FR-21 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Structures the separate B2B data entity independent of retail customers ensuring non-interference in scale out strategies. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Transaction
**Group:** Chapter 7: Data Entity
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-22 |
| **Requirement name** | Customer Order Transaction |
| **Requirement description** | There shall be a function to create a Customer Order transaction for a Customer. Each Customer Order shall contain the following information: customer details, shippingAddress, items, shippingFee, totalAmount, status, createdAt. A Customer Order is an immutable event record. Each Customer Order is uniquely identified by its ObjectId. A Customer Order is deemed to have happened when the order is successfully created. |
| **Source** | Data |
| **Owner** | Finance |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | DB |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Never modifies historical prices. |
| **Related requirements** | FR-04, FR-30 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Ensures an immutable transaction ledger capturing precise prices natively preserving historical auditing capabilities securely. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Transaction
**Group:** Chapter 7: Data Entity
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-23 |
| **Requirement name** | Client Payment Transaction |
| **Requirement description** | There shall be a function to create a Factory Client Payment transaction for a Factory Client. Each Factory Client Payment shall contain the following information: amount, clientId, effect (increments FactoryClient.paidAmount). A Factory Client Payment is a financial event. Each Factory Client Payment is uniquely identified by its ObjectId. A Factory Client Payment is deemed to have happened when recorded by the Owner. |
| **Source** | Data |
| **Owner** | Finance |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | B2B |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Modifies paidAmount. |
| **Related requirements** | FR-28 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Defines the event structure for receiving financial B2B payments executing immediately bypassing transaction queues dynamically. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Accessibility
**Group:** Chapter 8: User Function
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-24 |
| **Requirement name** | Colour Blind Accessibility |
| **Requirement description** | The user interface shall be accessible by people with specific colour blindness needs to the extent that color alone shall not be used as the sole means of identifying product colour options. Each colour swatch button shall include title='{Arabic colour name}'. |
| **Source** | UX |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | UI |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Title attributes present. |
| **Related requirements** | NFR-09, FR-43 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Requires tooltips to ensure non-visual distinction of product colors aligning precisely with WCAG usability requirements reliably. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Response time
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-25 |
| **Requirement name** | API Response Time |
| **Requirement description** | Each Core Commerce API Endpoint shall have a response time of no more than 500 ms for 95th percentile from request to response. The motivation for this requirement is to mandate strict latency thresholds for core commerce operations. |
| **Source** | Perf |
| **Owner** | Tech Lead |
| **Author** | DevOps |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | UX |
| **Stakeholders** | All |
| **Associated non functional requirements** | NFR-27 |
| **Acceptance criteria** | Load tests pass. |
| **Related requirements** | NFR-01 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Mandates strict latency thresholds for core commerce operations to sustain the retail user interaction flows securely. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Response time
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-26 |
| **Requirement name** | VTO AI Processing |
| **Requirement description** | Each Virtual Try-On AI image generation shall have a response time of no more than 15 seconds from request to response when HuggingFace is available. When HuggingFace is unavailable (quota/timeout > 30 s): return mock image in 2 s. |
| **Source** | Perf |
| **Owner** | Tech Lead |
| **Author** | API |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | AI |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Timeout strictly enforced. |
| **Related requirements** | FR-02 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Sets hard timeouts for asynchronous AI operations with fallbacks effectively blocking hanging tasks endlessly. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Throughput
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-27 |
| **Requirement name** | Concurrent API Throughput |
| **Requirement description** | The Fluffy backend shall be able to handle API requests transactions at a rate of at least 100 per second under peak load, leveraging Node.js non-blocking I/O. |
| **Source** | Perf |
| **Owner** | DevOps |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Scale |
| **Stakeholders** | Ops |
| **Associated non functional requirements** | NFR-25 |
| **Acceptance criteria** | Passes Artillery load test. |
| **Related requirements** | NFR-01 |
| **Related documents** | server.js |
| **comments** | Ensures the system can handle traffic spikes without degradation utilizing efficient node threading reliably. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Dynamic capacity
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-28 |
| **Requirement name** | Server RAM Utilisation |
| **Requirement description** | The system shall be able to satisfy a maximum of 512 MB simultaneous Server RAM consumption under sustained normal load. Uploaded images must not be buffered in RAM. |
| **Source** | Perf |
| **Owner** | DevOps |
| **Author** | Backend |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Infra |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | NFR-41 |
| **Acceptance criteria** | No memory leaks on upload. |
| **Related requirements** | NFR-41 |
| **Related documents** | server.js |
| **comments** | Restricts memory buffering to maintain stability under load by delegating processing to direct proxy streaming seamlessly. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Dynamic capacity
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-29 |
| **Requirement name** | Maximum Active Users |
| **Requirement description** | The system shall be able to satisfy 100 simultaneous active user sessions without response time exceeding established targets. |
| **Source** | Perf |
| **Owner** | DevOps |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | Scale |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-44 |
| **Acceptance criteria** | Stateless sessions support this. |
| **Related requirements** | FR-57 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Requires stateless session mechanisms to handle concurrent loads eliminating RAM requirements securely logically. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Dynamic capacity
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-30 |
| **Requirement name** | Payload Size Limit |
| **Requirement description** | The system shall be able to satisfy a maximum of 50 MB simultaneous HTTP Request Body payloads per individual request. The backend shall reject any request body exceeding 50 MB. |
| **Source** | Perf |
| **Owner** | Sec Lead |
| **Author** | Backend |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Sec |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | NFR-41 |
| **Acceptance criteria** | Rejects > 50MB. |
| **Related requirements** | NFR-01 |
| **Related documents** | server.js |
| **comments** | Sets constraints to accept image payloads while protecting against overflow attacks consistently effectively explicitly. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Static capacity
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-31 |
| **Requirement name** | Order Static Capacity |
| **Requirement description** | The system shall be able to handle a minimum of 200,000 Customer Order records. GET /orders query time ≤ 500 ms p95. |
| **Source** | Data |
| **Owner** | DBA |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | DB |
| **Stakeholders** | Admins |
| **Associated non functional requirements** | NFR-25 |
| **Acceptance criteria** | Queries remain fast. |
| **Related requirements** | NFR-43 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Specifies minimum data storage capabilities before architectural changes are needed supporting extended lifetime logically gracefully. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Availability
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-32 |
| **Requirement name** | System Uptime |
| **Requirement description** | The system shall normally be available to users ≥ 99.5% of each calendar month. "Normally available" shall be taken to mean Core Commerce API endpoints returning HTTP 200. |
| **Source** | SRE |
| **Owner** | Tech Lead |
| **Author** | DevOps |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Reliability |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-48 |
| **Acceptance criteria** | External monitor tracks it. |
| **Related requirements** | FR-18 |
| **Related documents** | server.js |
| **comments** | Quantifies operational availability metrics enabling strict alerts preserving business continuity efficiently safely. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Scalability
**Group:** Chapter 10: Flexibility
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-33 |
| **Requirement name** | Catalogue Scalability |
| **Requirement description** | The system shall be scalable to accommodate an unrestricted number of Products. Tens of thousands of products. When product count exceeds 200, pagination must be implemented. The motivation for this requirement is planned expansion to new clothing lines and seasonal collections. |
| **Source** | Strategy |
| **Owner** | PO |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | DB |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Queries use skip/limit. |
| **Related requirements** | FR-31 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Requires structural design allowing seamless addition of tens of thousands of items without application breakdown directly logically. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Extendability
**Group:** Chapter 10: Flexibility
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-34 |
| **Requirement name** | Payment Extendability |
| **Requirement description** | It shall be possible to extend the Online payment gateway module by developing and "plugging in" an extra software module. The introduction of any such module shall not require fundamental changes to the core Order schema or checkout flow to allow its introduction. |
| **Source** | Strategy |
| **Owner** | PO |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Low |
| **Business area** | Integration |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | NFR-22 |
| **Acceptance criteria** | Schema prepared. |
| **Related requirements** | NFR-22 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Ensures schemas support future integrations natively without cascading migrations blocking product launches flexibly actively. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Multi-lingual
**Group:** Chapter 10: Flexibility
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-35 |
| **Requirement name** | Multi-Lingual Support |
| **Requirement description** | The system shall be capable of operating in the following languages: Arabic and English. All user-facing text strings shall be externalised. Root HTML element shall support dir='rtl'/'ltr' toggling without breaking Tailwind CSS layouts. |
| **Source** | Strategy |
| **Owner** | PO |
| **Author** | Frontend |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | UI |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | NFR-02 |
| **Acceptance criteria** | Tailwind handles direction. |
| **Related requirements** | NFR-02 |
| **Related documents** | src/pages/Index.tsx |
| **comments** | Architectural necessity for supporting RTL and LTR content dynamically paving roads structurally easily. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Installability
**Group:** Chapter 10: Flexibility
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-36 |
| **Requirement name** | Installability |
| **Requirement description** | It shall be possible for the Fluffy Backend and Frontend to be installed by an Authorised system administrator. Installation shall require only setting env vars and running npm install && npm start. No source code modification required. |
| **Source** | Ops |
| **Owner** | DevOps |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Infra |
| **Stakeholders** | Admins |
| **Associated non functional requirements** | NFR-01 |
| **Acceptance criteria** | Uses .env only. |
| **Related requirements** | NFR-01 |
| **Related documents** | server.js |
| **comments** | Specifies that deployment configurations be separate from code via environment variables creating clean deployability. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Specific authorization
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-37 |
| **Requirement name** | Admin Dashboard Access |
| **Requirement description** | A user shall not access the Owner Dashboard UI (/dashboard routes) unless their role is 'owner' or 'admin'. |
| **Source** | Sec |
| **Owner** | Sec Lead |
| **Author** | API |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | Sec |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Returns 403. |
| **Related requirements** | FR-65 |
| **Related documents** | src/components/AdminRoute.tsx |
| **comments** | Requires stringent specific authorization rules to access business data separating internal actions completely. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Specific authorization
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-38 |
| **Requirement name** | Factory Portal Access |
| **Requirement description** | A user shall not access the Factory Dashboard and /wholesale-orders endpoints unless they have a verified factory client session. |
| **Source** | Sec |
| **Owner** | Sec Lead |
| **Author** | API |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Sec |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | JWT customer denied. |
| **Related requirements** | FR-50, FR-58 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Prevents retail customers from loading B2B interfaces completely isolating tenant experiences securely robustly. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Specific authorization
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-39 |
| **Requirement name** | Wholesale Pricing Lock |
| **Requirement description** | A user shall not access the modification of pricePerPiece on any Wholesale Order unless their role is 'owner' or 'admin'. |
| **Source** | Sec |
| **Owner** | Sec Lead |
| **Author** | API |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Sec |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Stripped from requests. |
| **Related requirements** | FR-59 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Prevents the initiator of an order from defining their own pricing removing exploitation vectors perfectly. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Specific authorization
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-40 |
| **Requirement name** | Password Reset Authorization |
| **Requirement description** | A user shall not access the password reset endpoint unless they provide a valid, unexpired reset token. |
| **Source** | Sec |
| **Owner** | Sec Lead |
| **Author** | API |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Sec |
| **Stakeholders** | Users |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Validates expiry. |
| **Related requirements** | NFR-42, FR-48 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Enforces specialized token checking without relying on standard JWT authentication cleanly validating temporally. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data longevity
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-41 |
| **Requirement name** | VTO Data Zero Retention |
| **Requirement description** | Customer VTO photos and AI-generated results shall be stored In-memory only for Zero seconds from the moment the API response is returned to the client browser. When data is eligible for removal, discarded immediately. Privacy protection. |
| **Source** | Legal |
| **Owner** | Compliance |
| **Author** | Sec |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | Privacy |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Garbage collected. |
| **Related requirements** | NFR-28, FR-44 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Ensures extreme privacy by mandating ephemeral handling of user media complying natively with regulations. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data longevity
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-42 |
| **Requirement name** | Password Token Longevity |
| **Requirement description** | Password Reset Token and expiry timestamp shall be stored Online in MongoDB User document for Exactly 10 minutes from the moment of token generation. When data is eligible for removal, token is considered expired. Security protection against replay attacks. |
| **Source** | Sec |
| **Owner** | Sec Lead |
| **Author** | Auth |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Sec |
| **Stakeholders** | Users |
| **Associated non functional requirements** | NFR-40 |
| **Acceptance criteria** | Erased after 10m. |
| **Related requirements** | NFR-40 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Limits the attack surface by enforcing strict temporal constraints on reset tokens actively removing risks seamlessly. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data longevity
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-43 |
| **Requirement name** | Financial Records Longevity |
| **Requirement description** | Customer Order records and Factory Client Payment records shall be stored Online in MongoDB for Minimum 5 years from the date of final status update. When data is eligible for removal, permanent deletion is prohibited before this window elapses. Legal and accounting requirements. |
| **Source** | Finance |
| **Owner** | Finance |
| **Author** | DBA |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Audit |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Blocks hard delete. |
| **Related requirements** | FR-61 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Blocks hard deletion to comply with financial auditing regulations preserving the historical source accurately. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data longevity
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-44 |
| **Requirement name** | Browser Session Longevity |
| **Requirement description** | User JWT and decoded profile shall be stored Client-side in browser localStorage for 90 days from the moment of successful login. When data is eligible for removal, expired JWTs shall be rejected. Balancing security against user convenience. |
| **Source** | UX |
| **Owner** | Sec |
| **Author** | Frontend |
| **Type of requirement** | Non-Functional |
| **Priority** | Medium |
| **Business area** | UX |
| **Stakeholders** | Users |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Fails smoothly when expired. |
| **Related requirements** | FR-57 |
| **Related documents** | src/components/AdminRoute.tsx |
| **comments** | Determines exactly how long clients remain authenticated without re-prompting balancing convenience and session rotation. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data longevity
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-45 |
| **Requirement name** | Worker Records Longevity |
| **Requirement description** | Worker document for departed employees shall be stored Online in MongoDB (flagged inactive) for Minimum 5 years from the date the worker was deactivated. When data is eligible for removal, permanent deletion is prohibited before 5 years. Labour law compliance. |
| **Source** | HR |
| **Owner** | HR |
| **Author** | DBA |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Audit |
| **Stakeholders** | Owner |
| **Associated non functional requirements** | NFR-49 |
| **Acceptance criteria** | isActive=false flag used. |
| **Related requirements** | NFR-49 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Mandates soft-deletes to preserve historical employment ledgers serving as compliance defense seamlessly dynamically. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Multi-organization unit
**Group:** Chapter 12: Commercial
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-47 |
| **Requirement name** | B2B Tenancy |
| **Requirement description** | The system shall support multiple Factory Clients. For the purpose of this specification, a Factory Client is an independent legal business on whose behalf the Atelier conducts wholesale manufacturing. Segregated wholesale orders (filtered by clientId); dedicated totalDebt and paidAmount. One system installation may serve dozens of partner factories. |
| **Source** | Arch |
| **Owner** | Tech Lead |
| **Author** | Architect |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | Scale |
| **Stakeholders** | Factories |
| **Associated non functional requirements** | NFR-38 |
| **Acceptance criteria** | Queries filtered by session. |
| **Related requirements** | FR-34 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Ensures strict data partitioning across multiple wholesale clients providing a multi-tenant environment natively. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Availability
**Group:** Chapter 9: Performance
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-48 |
| **Requirement name** | Graceful Degradation |
| **Requirement description** | The system shall normally be available to users at all times for the Product Detail page. "Normally available" shall be taken to mean the page must never crash or render a blank screen, relying on a two-tier fallback if the primary API fails. |
| **Source** | UX |
| **Owner** | Tech Lead |
| **Author** | Frontend |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Resilience |
| **Stakeholders** | Customers |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Renders without crashing. |
| **Related requirements** | NFR-32, FR-32 |
| **Related documents** | src/pages/ProductDetail.tsx |
| **comments** | Specifies fallback routines for retrieving critical display data shielding end users from backend instabilities elegantly. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Specific authorization
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-49 |
| **Requirement name** | Delete Operations Protection |
| **Requirement description** | A user shall not access the execution of DELETE /workers/:id and DELETE /factory-clients/:id unless their role is 'owner'. |
| **Source** | Sec |
| **Owner** | Sec Lead |
| **Author** | API |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | Sec |
| **Stakeholders** | Admin |
| **Associated non functional requirements** | NFR-45 |
| **Acceptance criteria** | Admin gets 403. |
| **Related requirements** | NFR-37 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Limits the ability to remove records exclusively to the highest administrative tier isolating dangerous functions systematically. |
| **Version history** | 1.0 |

<br>

### Pattern Type: Data type
**Group:** Chapter 6: Information
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-50 |
| **Requirement name** | Image Upload Multi-Part Handler |
| **Requirement description** | Image Upload Payloads, which are used for product definitions and VTO queries, shall be of the form multi-part form-data payloads processed as binary memory streams. Must be validated and mapped into Gradio-compatible handleFiles components before transmitting to external AI interfaces. |
| **Source** | Tech |
| **Owner** | Architect |
| **Author** | Backend |
| **Type of requirement** | Non-Functional |
| **Priority** | High |
| **Business area** | Data |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Successfully intercepts multipart binary streams via server upload middlewares. |
| **Related requirements** | FR-02, FR-44 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Crucial for capturing physical image streams via Multer and transforming them safely into the exact structured file objects demanded by the HuggingFace Gradio SDK. |
| **Version history** | 1.1 |

<br>

### Pattern Type: Specific authorization
**Group:** Chapter 11: Access Control & Approval
| Characteristic | Description |
| :--- | :--- |
| **Requirement identifier** | NFR-51 |
| **Requirement name** | Token-Based Authentication Middleware |
| **Requirement description** | A user shall not access protected endpoints unless they provide a valid Bearer JWT in the Authorization header. |
| **Source** | Security |
| **Owner** | Sec Lead |
| **Author** | API |
| **Type of requirement** | Non-Functional |
| **Priority** | Critical |
| **Business area** | Auth |
| **Stakeholders** | Devs |
| **Associated non functional requirements** | - |
| **Acceptance criteria** | Blocks requests without valid tokens. |
| **Related requirements** | FR-57, FR-65 |
| **Related documents** | COMPLETE_BACKEND_CODE.js |
| **comments** | Serves as the primary gatekeeper for the entire application's backend architecture defending against unauthenticated access. |
| **Version history** | 1.0 |
