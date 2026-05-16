
## 1. User Registration Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Registration |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | يتكرر عند إنشاء أي حساب جديد في النظام (Client, Worker, Admin). |
| **Classifications** | Functional: Yes; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتسجيل حساب جديد لـ (عميل B2C، أو عامل إنتاج، أو مدير الأتيليه) لإنشاء سجل هوية رقمي وصلاحيات فريدة.
* **When NOT to use:** لا يُستخدم لتسجيل الكيانات والمصانع الشريكة B2B (لأنهم يتبعون نمط Factory Client Management).

### [3] Discussion
يعالج هذا النمط جمع البيانات الأساسية للتفريق بين الأدوار المختلفة في السيستم. الكود الفعلي في موديل `User.js` يعتمد على حقل الـ `role` لتحديد مسار المستخدم لاحقاً.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **name** | إلزامي (Mandatory) | الاسم الكامل للمستخدم، نوع البيانات نصي String. |
| **email** | إلزامي (Mandatory) | البريد الإلكتروني الفريد (المعرف الأساسي)، ويتم تحويله لـ lowercase برمجياً. |
| **password** | إلزامي (Mandatory) | كلمة المرور، ويشترط الكود ألا تقل عن 6 أحرف قبل الحفظ. |
| **role** | إلزامي (Mandatory) | نوع الحساب ويأخذ حصراً: client أو worker أو admin. |

### [5] Templates
"يجب على نظام [Fluffy] إتاحة واجهة لتسجيل حساب جديد لـ [نوع المستخدم] عن طريق تقديم [الاسم، البريد الإلكتروني، وكلمة المرور]، وحفظ البيانات في جدول [User] بعد التحقق من عدم تكرار [البريد الإلكتروني]."

### [6] Examples
* **مثال 1:** يقوم العميل بالتسجيل عبر صفحة `SignUp.tsx` بإدخال البريد الإلكتروني وكلمة المرور ليكون دوره `client`.
* **مثال 2:** يقوم الأدمن بتسجيل عامل جديد عبر لوحة التحكم وتعيين دور `worker` له.

### [7] Development & Testing Considerations
* **للمطور:** يجب استخدام `bcryptjs` لتشفير كلمة المرور داخل الميدل وير `UserSchema.pre('save')` قبل التخزين.
* **المختبر:** اختبار إرسال طلب تسجيل بإيميل مسجل مسبقاً، والتحقق من استقبال الرد 400 Bad Request مع رسالة "User already exists".

### [8] Extra Requirements
* **متطلب تابع (Follow-on):** بمجرد نجاح تسجيل الحساب، يتم إرسال بريد إلكتروني ترحيبي آلياً باستخدام واجهة Nodemailer (i3).

---

## 2. User Authentication Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Authentication |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | يُستدعى عند كل محاولة تسجيل دخول للمنصة. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** عندما يحاول العميل أو العامل تسجيل الدخول عبر واجهة الـ `Login.tsx` للوصول إلى بياناته المحمية.
* **When NOT to use:** لا يُستخدم لفحص أدوار وصلاحيات المستخدم بعد الدخول (تلك وظيفة نمط الـ Authorization).

### [3] Discussion
يعتمد نظام الدخول في ملف `authController.js` على مطابقة البريد الإلكتروني، وفحص كلمة المرور المفتوحة مع الـ Hash المشفر، وفي حال النجاح يتم توليد رمز توثيق رقمي مُوقع برمجياً JWT Token يحتوي على الـ `id` والـ `role` لضمان أمان الجلسة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **email** | إلزامي (Mandatory) | البريد الإلكتروني المدخل للبحث عن سجل المستخدم. |
| **password** | إلزامي (Mandatory) | كلمة المرور المدخلة لمطابقتها مع قاعدة البيانات. |
| **token** | إلزامي (Mandatory) | رمز الـ JWT المتولد لإدراجه في الـ Headers للطلبات القادمة. |

### [5] Templates
"يجب على النظام عند استقبال طلب تسجيل الدخول، التحقق من صحة [البريد الإلكتروني] ومطابقة [كلمة المرور]، وتوليد [JWT Token] صالح وصادر من السيرفر."

### [6] Examples
* **مثال 1:** يقوم العميل بإدخال إيميله وباسورده في صفحة الدخول، فيقوم السيستم بإرجاع التوكين وحفظه في الـ AuthContext.

### [7] Development & Testing Considerations
* **للمطور:** يجب تخزين مفتاح التوقيع الرقمي للتوكين داخل متغير بيئي محمي ومخفي `process.env.JWT_SECRET` لضمان عدم تزويره.
* **المختبر:** التحقق من أن محاولة الدخول ببيانات مغلوطة تعيد الرد 400 ومصحوبة برسالة خطأ صلبة "Invalid credentials" لحماية أمان السيستم.

---

## 3. Specific Authorization Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Specific Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | يُطبق كـ Middleware على جميع الـ API routes الحساسة لحماية البيانات. |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لحظر لوحات التحكم الإدارية والـ Endpoints الحساسة عن المستخدمين العاديين (مثل منع العميل من الدخول لبيانات مصنع أو التلاعب بالمنتجات).
* **When NOT to use:** لا يُستخدم للتأكد من هوية المستخدم الحالية، بل يعمل بعد الـ Authentication للتأكد من الـ Role فقط.

### [3] Discussion
في كود مشروع Fluffy، يتم استخدام الميدل وير لفحص دور المستخدم برمجياً قبل تمرير الطلب. على سبيل المثال، المسارات البرمجية المخصصة لتعديل المنتجات أو الاطلاع على الرواتب محمية بفحص صارم لدور الـ `admin` أو `owner` لحماية السيستم.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **userRole** | إلزامي (Mandatory) | الدور الفعلي المستخرج برمجياً من الـ JWT الممرر للتحقق منه. |
| **requiredRole** | إلزامي (Mandatory) | الدور المطلوب والمصرح له بعبور المسار (مثل: admin). |
| **failureAction** | إلزامي (Mandatory) | الإجراء البرمجي عند الفشل (إرجاع كود الحظر الحاسم 403 Forbidden). |

### [5] Templates
"يجب على النظام منع الوصول إلى [المسار / لوحة التحكم] وحجب البيانات تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق [الدور المصرح له]."

### [6] Examples
* **مثال 1:** مسار الـ API لإضافة منتج جديد `/api/products` محمي برمجياً ومتاح فقط لمن يملك دور الـ `admin`.
* **مثال 2:** في الـ Frontend، شاشة الـ `OwnerDashboard.tsx` محمية بمكون الـ `AdminRoute.tsx` الذي يحول العميل تلقائياً لصفحة الدخول إذا حاول اختراق الرابط يدوياً.

### [7] Development & Testing Considerations
* **للمطور:** كتابة Middleware مخصص يقوم بقراءة الـ JWT وفحص الأدوار قبل الـ `next()`.
* **المختبر:** محاولة الدخول لحساب الأدمن باستخدام توكين مسجل كـ client والتحقق من استقبال رد المنع والحظر التام.

---

## 4. Product Inventory Maintenance Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Product Inventory Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | متكرر جداً (يُستدعى عند إضافة أو تعديل بضائع الأتيليه والخامات كالجلود). |
| **Classifications** | Functional: Yes; Affects database: Yes |

### [2] Applicability
* **When to use:** عندما يقوم الأدمن بإدارة منتجات المتجر الإلكتروني (إضافة فستان سواريه، تعديل خامة جلدية، تحديث الأسعار أو كمية الاستوك).
* **When NOT to use:** لا يُسخدم لتصفح المنتجات من قبل الزوار، حيث ينحصر فقط في العمليات التعديلية والإدارية (CRUD).

### [3] Discussion
يمتلك المنتج خصائص فنية دقيقة في قاعدة البيانات تم وصفها بموديل `Product.js`. النمط يضمن صرامة مدخلات المخازن والأسعار لمنع الأخطاء الحسابية أو تصفير الكميات بالخطأ.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **name** | إلزامي (Mandatory) | الاسم التجاري الفريد للقطعة، نوع البيانات String نصي. |
| **price** | إلزامي (Mandatory) | سعر القطعة المالي، ويشترط الكود رقماً موجباً أكبر من الصفر. |
| **stock** | إلزامي (Mandatory) | عدد القطع المتاحة في الورشة والمخزن (نوع رقمي Number). |
| **category** | إلزامي (Mandatory) | تصنيف المنتج في المتجر (مثل: Tops, Bottoms, Leather). |
| **images** | اختياري (Optional) | مصفوفة روابط صور المنتج المرفوعة لعرضها في الموقع والـ VTO. |

### [5] Templates
"يجب على النظام تمكين المسؤول من إجراء [إضافة / تعديل / حذف] على سجلات جدول [Product] بشرط أن تكون قيم حقول [price] وحقول [stock] موجبة تماماً."

### [6] Examples
* **مثال 1:** يقوم الأدمن بفتح مودال `AddProductModal.tsx` وإضافة منتج باسم "جاكيت جلد أسود" بسعر 2500 جنيه ومخزون 10 قطع.

### [7] Development & Testing Considerations
* **للمطور:** كتابة Mongoose Validation داخل موديل الـ Product يمنع حفظ أي قيمة سالبة في الأسعار برمجياً تلقائياً.
* **المختبر:** محاولة إرسال طلب تحديث منتج بـ سعر سالب (مثل price: -50) والتحقق من رفض السيستم للعملية وإرجاع رسالة خطأ.

---

## 5. Factory Worker Profile Maintenance Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Factory Worker Profile Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | يُستدعى عند تسجيل عامل جديد بالورشة أو تحديث مهاراته ورواتبه الحسابية. |
| **Classifications** | Functional: Yes; Affects database: Yes |

### [2] Applicability
* **When to use:** عندما تقوم الإدارة بتهيئة ملفات العمال لتحديد تخصصاتهم الفنية ورواتبهم الأساسية لتوزيع المهام بكفاءة داخل خط إنتاج الأتيليه.
* **When NOT to use:** لا يُستخدم لرصد حضور وغياب العمال اليومي، بل ينحصر في إدارة الحسابات المالية والملفات الأساسية.

### [3] Discussion
يعتمد توزيع الأوردرات اللوجستية في الورشة على مهارة العامل. يسجل جدول `Worker.js` معرّف العامل بربطه بحسابه الأصلي في الـ User ويحفظ حقول مصفوفة المهارات `skills` ومعدل الراتب الأساسي والخصومات لإجراء عمليات كشف الرواتب بدقة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **user** | إلزامي (Mandatory) | رابط مرجعي يربط العامل بجدول الـ User العام (ObjectId). |
| **skills** | إلزامي (Mandatory) | مصفوفة تحتوي على المهارات (مثل: Sewing, Cutting, Embroidering). |
| **salary** | إلزامي (Mandatory) | الراتب الشهري المتفق عليه، وهو رقم موجب يُستخدم في حسابات الرواتب صافية. |
| **deductions** | إلزامي (Mandatory) | الخصومات والجزاءات المالية المطبقة على العامل للشهر الحالي. |

### [5] Templates
"يجب على النظام السماح للمسؤول بإدارة وتعديل ملفات العمال بربطها بـ [معرّف المستخدم]، وتدوين مصفوفة [المهارات]، وحفظ الأجر الأساسي في حقل [salary]."

### [6] Examples
* **مثال 1:** تقوم الإدارة بفتح مودال `AddWorkerModal.tsx` لتسجيل عامل جديد وتحديد راتبه بـ 6000 جنيه شهرياً وإضافة مهارة "خياطة الجلود".

### [7] Development & Testing Considerations
* **للمطور:** يجب استخدام خاصية الـ `populate('user')` عند جلب بيانات العمال لعرض أسمائهم وبياناتهم الأساسية بشكل مترابط في الـ Frontend.
* **المختبر:** التحقق من أن خوارزمية حساب صافي المرتب تعطي ناتجاً صحيحاً يطابق معادلة الكود: `NetSalary = Salary - Deductions`.

---

## 6. Factory Client Management Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Factory Client Management |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | يُستدعى مرة واحدة عند تسجيل مصنع أو أتيليه شريك جديد في تعاقدات الجملة B2B. |
| **Classifications** | Functional: Yes; Affects database: Yes |

### [2] Applicability
* **When to use:** عندما ترغب إدارة الأتيليه في تسجيل بيانات الكيانات والمصانع الخارجية الشريكة لمتابعة طلبيات الجملة والديون والمدفوعات.
* **When NOT to use:** لا يُستخدم لتسجيل حسابات العملاء الأفراد B2C الذين يشترون قطعاً فردية من الماركت الإلكتروني.

### [3] Discussion
يتعامل السيستم مع الكيانات التجارية بشكل منفصل عن الأفراد. موديل `FactoryClient.js` مصمم خصيصاً لحفظ اسم الشركة، المسؤول، رقم التليفون، والـ `totalDebt` (إجمالي الديون) والـ `paidAmount` لتنظيم المعاملات المالية الكبرى وحسابات التعاقد.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **companyName** | إلزامي (Mandatory) | الاسم التجاري الرسمي للمصنع أو الأتيليه الشريك (String). |
| **ownerName** | إلزامي (Mandatory) | اسم المالك أو الشخص المسؤول عن التوقيع وإدارة التعاقد والطلب. |
| **username** | إلزامي (Mandatory) | اسم مستخدم فريد لتمكين المصنع الشريك من الدخول لبوابته الرقمية. |
| **totalDebt** | إلزامي (Mandatory) | القيمة المالية الكلية المستحقة على المصنع وتزيد ديناميكياً مع كل أوردر. |

### [5] Templates
"يجب على النظام السماح للأدمن بتهيئة وإدارة حسابات الكيانات الشريكة عن طريق تسجيل [اسم الشركة]، وتعيين [اسم المالك للمصنع] وحفظ الديون في حقل [totalDebt]."

### [6] Examples
* **مثال 1:** تسجل الإدارة عبر واجهة `AddClientModal.tsx` حساباً لـ "مصنع سادا للملابس" برصيد ديون يبدأ من صفر.
* **مثال 2:** يقوم العميل التجاري بالدخول لـ `FactoryClientPortal.tsx` لمراجعة فواتير الديون والطلبيات الخاصة به.

### [7] Development & Testing Considerations
* **للمطور:** كتابة دالة في الـ Backend بملف `clientController.js` تقوم بزيادة حقل الـ `paidAmount` وإنقاص الدين آلياً عند تسجيل عملية دفع كاش.
* **المختبر:** التحقق من أن النظام يمنع تكرار تسجيل اسم مستخدم `username` مسجل مسبقاً لمنع تداخل حسابات الكيانات التجارية.

---

## 7. Production Task Scheduling Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Production Task Scheduling |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | يُستدعى ديناميكياً فور إنشاء طلب تصنيع ملابس أو تأكيد أوردر تفصيل. |
| **Classifications** | Functional: Yes; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لجدولة وتوزيع الأوردرات داخل الورشة وتكليف العمال ومتابعة تغير حالات الأوردر اللوجستية حتى التسليم النهائي للعميل.
* **When NOT to use:** لا يُستخدم لمعالجة المدفوعات المالية الفورية لمنتجات الموقع الجاهزة التي لا تحتاج لتصنيع في الورشة.

### [3] Discussion
يمثل هذا النمط عصب اللوجستيات في كود مشروعك (`OrderService.js` وموديل `Order.js`). يراقب النمط حالات الأوردر ويمنع التأخر في المواعيد النهائية المحددة للتسليم عبر ربط المهام التشغيلية بـ العمال الأكفاء لها في الورشة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **products** | إلزامي (Mandatory) | مصفوفة تحتوي على تفاصيل الملابس، الألوان، والمقاسات المطلوبة. |
| **workerId** | اختياري (Optional) | معرّف العامل المكلف بتنفيذ هذا الأمر في الورشة (ObjectId). |
| **status** | إلزامي (Mandatory) | حالة الإنتاج وتأخذ قيم ثابتة: Pending أو In Progress أو Completed. |
| **dueDate** | إلزامي (Mandatory) | التاريخ الأقصى المسموح به لإنهاء تسليم القطعة تفادياً للتأخير. |

### [5] Templates
"يجب على النظام عند إنشاء أمر تشغيل، إسناده لـ [العامل المكلف] وتحديث حالة الإنتاج في حقل [status] لتنتهي القطعة قبل تاريخ [dueDate]."

### [6] Examples
* **مثال 1:** يقوم مدير الورشة عبر الـ `AddProductionModal.tsx` بإنشاء أمر تفصيل لجاكيت وإسناده للعامل "سيد الخياط" وحفظه كـ `In Progress`.
* **مثال 2:** يستعرض العامل مهامه النشطة في صفحة `FactoryDashboard.tsx` ويقوم بضغط زر الإكمال لتتحول الحالة إلى `Completed`.

### [7] Development & Testing Considerations
* **للمطور:** يجب منع الانتقال البرمجي العشوائي للحالات داخل `OrderService.js` (منع الانتقال من Pending إلى Completed مباشرة دون المرور بـ In Progress) لضمان صحة البيانات اللوجستية.
* **المختبر:** التحقق من أن واجهة العامل لا تظهر له ولا تتيح له تعديل سوى الأوردرات المسندة لمعرّفه الشخصي فقط منعاً للتداخل.

---

## 8. Virtual Try-On (VTO) Image Processing Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Virtual Try-On (VTO) Image Processing |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | يُستدعى في كل مرة يطلب فيها العميل تجربة لباس افتراضي على جسده. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** عندما يقف العميل في صفحة الـ `VirtualTryOn.tsx` ويرفع صورته الشخصية مع اختيار فستان من الأتيليه لرؤية ملاءمة القطعة لجسده عبر دمج الذكاء الاصطناعي.
* **When NOT to use:** لا يُستخدم لعرض صور الموديلات الثابتة التقليدية، بل ينحصر في عمليات المعالجة التوليدية الفورية للعملاء.

### [3] Discussion
تعتبر هذه الميزة التنافسية الكبرى لـ Fluffy وتظهر في كود `vtoController.js` وملفات الـ Tests. يقوم النمط بإرسال طلب غير متزامن لنموذج الـ AI الخارجي (`fashn-ai/fashn-vton-1.5` أو Gradio) وانتظار التوليد الرقمي لإرجاع الصورة النهائية وعرضها للعميل.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **personImage** | إلزامي (Mandatory) | رابط أو ملف الصورة الشخصية المرفوعة من العميل (String/File). |
| **clothImage** | إلزامي (Mandatory) | رابط صورة قطعة اللباس المراد قياسها من الداتا بيس. |
| **resultUrl** | إلزامي (Mandatory) | رابط الصورة المعالجة والمدمجة النهائية الراجعة من خادم الـ AI بنجاح. |

### [5] Templates
"يجب على النظام عند طلب العميل تجربة لباس، إرسال [صورة الشخص] و [صورة اللباس] لواجهة معالجة الـ AI، واستقبال النتيجة في [resultUrl] لعرضها في الشاشة."

### [6] Examples
* **مثال 1:** ترفع العميلة صورتها وتختار فستاناً سواريه، يستدعي كود الـ Backend السيرفر الخارجي الموضح في `test_gradio_run.js` ويعرض الصورة المدمجة للعميلة في أقل من 15 ثانية.

### [7] Development & Testing Considerations
* **للمطور:** معالجة الـ AI تأخذ وقتاً، لذا يجب إحاطة الكود ببلوك `try/catch` قوي للتعامل مع حالات الـ Timeouts وسقوط الـ APIs الخارجية لضمان استقرار السيرفر.
* **المختبر:** التحقق من إظهار مؤشر تحميل (Spinner) في صفحة الـ Frontend أثناء المعالجة لحظر الضغط المتكرر من العميل وتوفير موارد السيرفر الكبيرة.

---

## 9. Customer Order Cost Calculation Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Customer Order Cost Calculation |
| **Domain** | Commercial Domain |
| **Group** | Financial Group |
| **Anticipated Frequency** | يُستدعى ديناميكياً عند كل ضغط لزر إتمام الشراء في شاشة الـ `Cart.tsx`. |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لحساب القيمة المالية الإجمالية للمشتريات داخل السلة بضرب الكميات في الأسعار الرسمية وإضافة تكاليف شحن المحافظات وتسجيلها نهائياً.
* **When NOT to use:** لا يُستخدم لحساب الديون الكبيرة للمصانع الشريكة B2B (لأن تلك وظيفة نمط Factory Client Invoicing).

### [3] Discussion
حماية العمليات المالية محورية لمنع ثغرات تعديل الأسعار الشهيرة في الـ Browser. الكود الفعلي في الـ Backend بملف `orderController.js` يستقبل فقط معرّفات المنتجات من الـ Frontend، ثم يقوم بعمل لووب برميجي وجلب الأسعار الحقيقية والصلبة من قاعدة البيانات لحساب الـ `totalAmount` بأمان تام.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **items** | إلزامي (Mandatory) | مصفوفة تحتوي على معرّفات المنتجات والكمية المحددة لكل قطعة. |
| **totalAmount** | إلزامي (Mandatory) | السعر الإجمالي الكلي المحسوب برمجياً من السيرفر ويحفظ كـ Record ثابت. |
| **shippingAddress** | إلزامي (Mandatory) | تفاصيل العنوان والمحافظة لحساب تكلفة الشحن المناسبة تلقائياً. |

### [5] Templates
"يجب على النظام عند إتمام عملية الدفع، احتساب حقل [totalAmount] بجمع أسعار المنتجات الحقيقية من قاعدة البيانات وإضافة مصاريف شحن الـ [Governorate]."

### [6] Examples
* **مثال 1:** يشتري عميل قطعتين، يقوم الكود في `orderController.js` بضرب الكمية في السعر وجلب قيمة الشحن، وتوليد الفاتورة برقم ObjectId فريد في جدول الطلبات.

### [7] Development & Testing Considerations
* **للمطور:** يجب حفظ الأسعار وقت الشراء كـ قيم رقمية ثابتة (Snapshot Prices) داخل سجل الأوردر، حتى لا تتأثر الفاتورة القديمة للعميل إذا قامت الإدارة بتغيير سعر المنتج في المتجر لاحقاً.
* **المختبر:** اختبار محاولة إرسال طلب شراء معدّل بالمتصفح (مثال: تمرير سعر قطعة بـ 5 جنيهات بدلاً من 500 جنيه)، والتحقق من أن الـ Backend يرفض القيمة المزيفة ويحتسب السعر الأصلي الصحيح من الداتا بيس مباشرة.

---

## 10. Factory Client Invoicing Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Factory Client Invoicing |
| **Domain** | Commercial Domain |
| **Group** | Financial Group |
| **Anticipated Frequency** | يُستدعى ديناميكياً عند تسليم طلبيات التصنيع الكبرى للمصانع والأتيليهات الشريكة. |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لإصدار الفواتير الكبرى وحساب الـ Debt (الديون الكلية) والمبالغ المدفوعة للتعاملات التجارية B2B بين الأتيليه والمصانع الشريكة.
* **When NOT to use:** لا يُستخدم لطلبات الشراء الفردية الصغيرة للعملاء الأفراد عبر موقع المتجر التجاري.

### [3] Discussion
تختلف فواتير الكيانات الكبيرة عن الأفراد؛ حيث تشمل حساب عقود وكميات ضخمة بناءً على عدد القطع والمقاسات. النمط يربط معرّف الـ `FactoryClient` بقائمة الأوردرات المصنعة له، ويقوم بحساب إجمالي السعر وإضافته لـ حقل الـ `totalDebt` الخاص بالمصنع الشريك لضمان سلامة الدورة الحسابية والمالية للمشروع.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **clientId** | إلزامي (Mandatory) | معرّف المصنع أو الأتيليه الشريك صاحب الأوردر (ObjectId يربط بـ FactoryClient). |
| **totalPrice** | إلزامي (Mandatory) | السعر الإجمالي للطلبية الكبيرة المحسوب بضرب (عدد القطع * سعر القطعة المتفق عليه). |
| **status** | إلزامي (Mandatory) | حالة الأوردر، وفور تحولها إلى "تم التسليم" يتم ترحيل المبلغ كـ دين لحساب المصنع. |

### [5] Templates
"يجب على النظام تجميع حساب طلبيات التصنيع لـ [المصنع الشريك]، وإصدار فاتورة إجمالية برقم فريد، وترحيل السعر لـ حقل [totalDebt] فور تسليم الطلبية."

### [6] Examples
* **مثال 1:** يقوم السيستم بتجميع حساب طلب تصنيع كمية معينة لـ "أتيليه سادا" وإصدار فاتورة تجارية كبرى وعرضها داخل بوابة العميل `FactoryClientPortal.tsx` لمراجعة ديونه.

### [7] Development & Testing Considerations
* **للمطور:** ربط الفواتير المفتوحة بحسابات المصانع برمجياً، بحيث يمنع السيرفر رفع طلب تصنيع جديد ضخم إذا كان العميل التجاري متجاوزاً للحد الائتماني المسموح به أو لديه مبالغ ديون متأخرة لم تسدد.
* **المختبر:** التحقق من زيادة الـ `totalDebt` بمقدار السعر الإجمالي للأوردر فوراً وتلقائياً عند تحويل حالة الأوردر لـ "تم التسليم" بنجاح.

---

## 11. Inquiry Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inquiry |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | من 2% إلى 15% من إجمالي المتطلبات. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتعريف شاشة أو واجهة تقوم بعرض معلومات محددة للمستخدم دون السماح بتعديلها في نفس الشاشة (Inquiry).
* **When NOT to use:** لا يُستخدم للتقارير المطبوعة أو التقارير الضخمة التي يتم توليدها في الخلفية (يُستخدم Report Pattern بدلاً منها).

### [3] Discussion
الاستعلامات (Inquiries) هي واجهة النظام الأساسية التي تتيح للمستخدمين رؤية البيانات. في مشروع Fluffy، توجد استعلامات عديدة مثل استعلام المنتجات (`Products List`) واستعلام الطلبات للعميل. يجب أن تكون الاستعلامات واضحة وتحتوي على معايير فرز (Sort) وتصفية (Filter) دقيقة لتسهيل الوصول للمعلومة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **inquiryName** | إلزامي (Mandatory) | اسم الاستعلام (مثل: Customer Orders Inquiry). |
| **businessIntent** | إلزامي (Mandatory) | الغرض التجاري من الاستعلام لتوجيه المطور للهدف الفعلي منه. |
| **informationToShow** | إلزامي (Mandatory) | الحقول المعروضة (مثل: Product Name, Price, Colors). |
| **selectionCriteria** | اختياري (Optional) | معايير الفلترة (مثل الفلترة بالـ Category في واجهة المنتجات). |

### [5] Templates
"يجب أن يوفر النظام استعلاماً باسم [inquiryName] لعرض [informationToShow]. الغرض منه هو [businessIntent]. يمكن تصفية البيانات باستخدام المعايير التالية: [selectionCriteria]."

### [6] Examples
* **مثال 1:** في صفحة `OwnerDashboard.tsx`، هناك استعلام `Recent Orders Inquiry` يعرض تفاصيل الطلبات، التكلفة الإجمالية، وحالة الطلب مرتبة من الأحدث للأقدم.
* **مثال 2:** استعلام بيانات العملاء التجاريين (Factory Clients) يعرض اسم الشركة وحجم المديونية.

### [7] Development & Testing Considerations
* **للمطور:** يجب مراعاة الـ Performance وعدم جلب بيانات ضخمة دفعة واحدة، بل يفضل استخدام الـ Pagination إن أمكن، بالإضافة إلى تطبيق الـ Authorization لمنع تسريب البيانات.
* **المختبر:** التحقق من أن تغيير معايير الفلترة (مثل تغيير الـ Category إلى Tops) يجلب البيانات الصحيحة فقط.

---

## 12. Report Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Report |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | من 2% إلى 15% من المتطلبات. |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم عند الحاجة لتجميع البيانات ومعالجتها وتقديمها كتقرير شامل (كثيراً ما يحتوي على عمليات حسابية ورسوم بيانية وإحصائيات)، بغرض الاستعراض الإداري والتحليل.
* **When NOT to use:** لا يُستخدم لعرض سجل واحد أو بيانات لحظية بسيطة للمستخدم العادي (تلك وظيفة الـ Inquiry).

### [3] Discussion
التقارير هي الأداة الأقوى لصناع القرار. في مشروع Fluffy، يبرز نمط التقارير بوضوح في مكون `AIPredictionsTab.tsx` الذي يحلل الطلبات والمبيعات لتقديم توقعات مبيعات الذكاء الاصطناعي (AI Predictions) والمنتجات الأكثر مبيعاً.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **reportName** | إلزامي (Mandatory) | اسم التقرير (مثل: AI Sales Predictions Report). |
| **businessIntent** | إلزامي (Mandatory) | الغرض التجاري (مثل: التنبؤ بإيرادات الشهر القادم). |
| **informationToShow** | إلزامي (Mandatory) | البيانات والإحصائيات المعروضة (مثل: Revenue Forecast, At-Risk Products). |
| **totalingLevels** | اختياري (Optional) | مستويات التجميع والإجماليات المطلوبة في نهاية التقرير. |

### [5] Templates
"يجب أن يوفر النظام تقريراً باسم [reportName] لعرض [informationToShow]. الغرض من التقرير هو [businessIntent]. يجب أن يتضمن التقرير إجماليات لـ [totalingLevels]."

### [6] Examples
* **مثال 1:** تقرير `AI Predictions` يحلل بيانات آخر 3 أشهر ويحسب إجمالي الإيرادات شهرياً، ثم يتوقع إيرادات الشهر القادم ويحلل الألوان والمقاسات الأكثر طلباً.

### [7] Development & Testing Considerations
* **للمطور:** يجب الحذر من أن التقارير المعقدة قد تسبب بطئاً (Performance Impact) في قاعدة البيانات الرئيسية، ويُفضل معالجتها بكفاءة عبر `Aggregation`.
* **المختبر:** تجهيز Test Data ببيانات مبيعات ضخمة للتأكد من أن التقرير يحسب الإجماليات بدقة ولا يتعطل أثناء التوليد.

---

## 13. Chronicle Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Chronicle |
| **Domain** | Data Entity Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | من 1 إلى 20 متطلباً حسب حساسية السيستم. |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتسجيل أي حدث (Event) هام في النظام يتم الاحتفاظ به كمرجع تاريخي (Audit Trail) لمعرفة ماذا حدث، متى، ومن قام به.
* **When NOT to use:** لا يُستخدم لتسجيل المعاملات المالية (Transactions) التي تتبع نمط Transaction Pattern.

### [3] Discussion
التسجيل الزمني أو السجل (Chronicle) يحمي النظام ويساعد في تتبع الأخطاء والتصرفات غير المصرح بها. في مشروعك، يمكن استخدامه لتسجيل الأخطاء (Errors) الناتجة من واجهة n8n أو محاولات الدخول الخاطئة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **occurrenceType** | إلزامي (Mandatory) | نوع الحدث المراد تسجيله (مثل: فشل إرسال Webhook). |
| **informationToRecord**| إلزامي (Mandatory) | تفاصيل الحدث (مثل: التاريخ، الوقت، الـ Error Message). |
| **severity** | اختياري (Optional) | مستوى الخطورة (مثل: Normal, Warning, Severe). |

### [5] Templates
"يجب على النظام تسجيل حدث [occurrenceType] تلقائياً عند وقوعه. يجب أن تتضمن البيانات المسجلة: [informationToRecord]، ويُصنف الحدث بمستوى خطورة [severity]."

### [6] Examples
* **مثال 1:** النظام يقوم بتسجيل أي خطأ ناتج أثناء محاولة إرسال بيانات الطلب إلى سيرفر n8n الخارجي (Webhook Failure) في ملف `orderController.js` لتسهيل مراجعته لاحقاً.

### [7] Development & Testing Considerations
* **للمطور:** يجب استخدام آلية سريعة غير متزامنة لتسجيل هذه الأحداث (مثل `console.error` المرتبط بـ Log File، أو حفظها في Database Collection منفصلة) لئلا تعطل مسار الكود الأساسي.
* **المختبر:** التحقق من قدرة النظام على تخزين الحدث بتوقيت دقيق (Timestamp) وتوافر معلومات كافية لتتبع أصل المشكلة (Stack Trace).

---

## 14. Data Type Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Type |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | يُستخدم لتحديد أنواع البيانات الأساسية المتكررة (أقل من 10 متطلبات عادة). |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتعريف نوع بيانات محدد ونمطه، لضمان تخزينه وعرضه بصيغة موحدة في جميع أجزاء النظام (مثل البريد الإلكتروني، المحافظات، أو أكواد الألوان).
* **When NOT to use:** لا يُستخدم للهياكل البيانية المعقدة جداً التي تتطلب جداول متعددة (يُستخدم Data Structure Pattern).

### [3] Discussion
البيانات في النظام تحتاج إلى معايير ثابتة لمنع الأخطاء. في Fluffy، توحيد حالة البريد الإلكتروني إلى حروف صغيرة (lowercase)، وتوحيد أسماء المحافظات وأكواد الألوان يعتبر تطبيقاً جوهرياً لنمط نوع البيانات (Data Type).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **dataTypeName** | إلزامي (Mandatory) | اسم نوع البيانات (مثل: Email Address). |
| **form** | إلزامي (Mandatory) | الهيئة البرمجية للبيانات (نص String، رقم Number). |
| **displayFormat** | اختياري (Optional) | كيفية عرض البيانات (مثل تحويل كود `#ff0000` إلى "أحمر"). |
| **constraints** | اختياري (Optional) | القيود (مثل أن يحتوي البريد على `@` ولا يقبل حروفاً كبيرة). |

### [5] Templates
"يجب أن يكون نوع البيانات [dataTypeName] بهيئة [form]. يجب أن تُعرض البيانات للمستخدم بصيغة [displayFormat]، مع تطبيق القيود التالية: [constraints]."

### [6] Examples
* **مثال 1:** البريد الإلكتروني (Email) في `authController.js` يُفرض أن يكون من نوع نصي (String) ويتم تطبيق قيد إجباري بتنفيذ دالة `.toLowerCase().trim()` عليه قبل الحفظ في قاعدة البيانات.
* **مثال 2:** كود الألوان (Color Code) يُحفظ بالـ Hex (مثل `#ff0000`) ولكن يُعرض للمستخدم بنصوص مفهومة (مثل "أحمر") عبر الخريطة البرمجية `getColorCode`.

### [7] Development & Testing Considerations
* **للمطور:** يجب وضع هذه القيود مركزياً (مثل `Mongoose Schemas` و الميدلوير `pre-save`) لضمان عدم تمرير بيانات مشوهة من الواجهة الأمامية بالخطأ.
* **المختبر:** إدخال بريد إلكتروني يحتوي على مسافات وحروف كبيرة ` Test@GMAIL.com `، والتحقق من حفظه بصيغة `test@gmail.com`.

---

## 15. ID Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | ID |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | من 1 إلى 6 متطلبات (لكل كيان أساسي). |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتحديد كيفية إنشاء المعرفات الفريدة (Unique Identifiers) للكيانات لمنع التكرار وتمييز كل سجل عن غيره.
* **When NOT to use:** لا يُستخدم للقيم العادية التي تقبل التكرار.

### [3] Discussion
المعرفات (IDs) هي المفاتيح الأساسية لقاعدة البيانات. في مشروع Fluffy، يستخدم MongoDB معرّفات تلقائية من نوع `ObjectId`. ولكن لأغراض عرض رقم الطلب (Order ID) للعميل، نحتاج لجعله مقروءاً وقصيراً.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **ownerEntityName**| إلزامي (Mandatory) | الكيان المالك للـ ID (مثل: Order, User). |
| **idForm** | إلزامي (Mandatory) | شكل الـ ID (مثل: 24-character hex string). |
| **scopeOfUniqueness**| إلزامي (Mandatory)| نطاق الفرادة (مثل: فريد على مستوى جدول الطلبات). |
| **howAllocated** | إلزامي (Mandatory) | كيفية تخصيصه (مثل: يتم توليده تلقائياً من MongoDB). |

### [5] Templates
"يجب أن يمتلك كل [ownerEntityName] معرّفاً فريداً بهيئة [idForm] يتم توليده بواسطة [howAllocated]. يجب أن يكون المعرّف فريداً على مستوى [scopeOfUniqueness]."

### [6] Examples
* **مثال 1:** يتم تخصيص `ObjectId` فريد كـ (Order ID) لكل طلب. وعند عرض رقم الطلب للعميل (في شاشة `OwnerDashboard`)، يتم استقطاع آخر 6 أحرف فقط وعرضها بأحرف كبيرة `String(order._id).slice(-6).toUpperCase()`.

### [7] Development & Testing Considerations
* **للمطور:** عند استخدام `ObjectId` المقتطع للعرض، لا تستخدمه للبحث في قاعدة البيانات، بل استخدم المعرّف الأصلي الكامل لضمان دقة البحث.
* **المختبر:** مراجعة شاشات الواجهة الأمامية للتأكد من أن معرّفات الطلبات الطويلة جداً لا تشوه تنسيق الجدول، وأن التنسيق المقتطع يعمل بنجاح.

---

## 16. Calculation Formula Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | تعتمد على طبيعة المشروع (من 0 إلى 12 متطلباً). |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتحديد كيفية حساب قيمة معينة رياضياً عبر مجموعة خطوات أو متغيرات لضمان ألا يقوم المطور باجتهادات خاطئة.
* **When NOT to use:** لا يُستخدم للمعادلات المعقدة جداً والمطولة التي تفضل إحالتها لمستند مرجعي منفصل.

### [3] Discussion
المعادلات المالية يجب أن تكون صارمة. في Fluffy، توجد معادلات هامة مثل حساب إجمالي فاتورة العميل، وحساب صافي راتب العامل بعد الخصومات.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | اسم القيمة المحسوبة (مثل: Total Order Amount). |
| **formula** | إلزامي (Mandatory) | المعادلة الرياضية المُنفذة للحصول على القيمة. |
| **variables** | إلزامي (Mandatory) | المتغيرات الداخلة في المعادلة (مثل: Price, Quantity, Shipping Fee). |

### [5] Templates
"يجب حساب [valueName] باستخدام المعادلة التالية: [formula]، حيث تمثل المتغيرات القيم الآتية: [variables]."

### [6] Examples
* **مثال 1:** يُحسب إجمالي الطلب للعميل كالتالي: `Total Amount = (ItemsTotal) + ShippingFee`، حيث `ItemsTotal` هو مجموع (الكمية `quantity` * السعر `price`) لكل منتج.
* **مثال 2:** يُحسب صافي الراتب للعامل في `OwnerDashboard.tsx` كالتالي: `NetSalary = Math.max(0, Salary - Deductions)`.

### [7] Development & Testing Considerations
* **للمطور:** يجب حساب هذه القيم من جانب السيرفر (Backend) في ملف `orderController.js` (باستخدام الميدلوير `pre('save')`)، ولا يتم الاعتماد مطلقاً على القيمة المرسلة من الواجهة الأمامية لحماية النظام من التلاعب.
* **المختبر:** التحقق من سيناريو وجود خصومات `Deductions` أعلى من الراتب `Salary` والتأكد من أن صافي الراتب لا يصبح بالسالب.

---

## 17. Data Longevity Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Longevity |
| **Domain** | Information Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | من متطلبين إلى 3 متطلبات. |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتحديد المدة التي يجب أن تحتفظ فيها قاعدة البيانات بسجلات معينة قبل السماح بحذفها أو إزالتها (لأغراض تنظيمية أو قانونية).
* **When NOT to use:** لا يُستخدم لتحديد مسار وتوقيت نقل البيانات (أرشفتها)، فتلك وظيفة (Data Archiving).

### [3] Discussion
الاحتفاظ بالبيانات يضمن حق المؤسسة والعميل، خاصة في البيانات المالية. في Fluffy، فواتير المبيعات (Retail Orders) وفواتير الجملة (Wholesale Orders) تعد سندات مالية يجب تخزينها لمدد محددة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **dataDescription**| إلزامي (Mandatory) | وصف البيانات المعنية (مثل: Customer Orders). |
| **mannerOfStorage**| إلزامي (Mandatory) | طريقة الحفظ (متصلة بقاعدة البيانات الأساسية Online). |
| **retentionDuration**| إلزامي (Mandatory) | مدة الاحتفاظ بالبيانات (مثل: 5 سنوات). |
| **durationStartTrigger**| إلزامي (Mandatory) | متى يبدأ العداد؟ (مثل: من تاريخ تسليم الطلب). |

### [5] Templates
"يجب تخزين بيانات [dataDescription] بصورة [mannerOfStorage] لمدة [retentionDuration] محسوبة من [durationStartTrigger]."

### [6] Examples
* **مثال 1:** يجب تخزين سجلات طلبات العملاء (Customer Orders) وفواتير المصانع بصورة (Online) في قاعدة البيانات لمدة (5 سنوات) من تاريخ تحديث حالة الطلب إلى `Delivered` أو `تم التسليم`.

### [7] Development & Testing Considerations
* **للمطور:** يجب منع وظيفة الـ Delete المباشرة (Hard Delete) للطلبات الناجحة في الـ Backend وتوفير بديل لتحديث حالتها (Soft Delete) فقط إن لزم الأمر.
* **المختبر:** محاولة طلب مسار الـ Delete API لطلب مكتمل، والتأكد من أن السيرفر يمنع العملية.

---

## 18. Data Archiving Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Archiving |
| **Domain** | Information Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | من 0 إلى 3 متطلبات. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتحديد آلية نسخ أو نقل البيانات المتقادمة من قاعدة البيانات النشطة إلى مخزن آخر (مثل Offline Storage أو Archive Database) لتقليل العبء على السيرفر الأساسي.
* **When NOT to use:** لا يُستخدم للـ Backups العادية للنظام والتي تتم يومياً (هذه وظيفة الـ Infrastructure).

### [3] Discussion
الطلبات الملغية (Cancelled Orders) والمرتجعات مع الوقت تستهلك مساحة. تحديد نمط لأرشفتها يضمن استقرار حجم قاعدة بيانات MongoDB الأساسية.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **dataDescription**| إلزامي (Mandatory) | وصف البيانات (مثل: الطلبات الملغية أو القديمة). |
| **moveOrCopy** | إلزامي (Mandatory) | نقل (حذف من المصدر) أو نسخ (Copy). |
| **dataDestination**| إلزامي (Mandatory) | وجهة الأرشيف (Offline Database). |
| **frequency** | إلزامي (Mandatory) | تكرار عملية الأرشفة (مثلاً: شهرياً). |

### [5] Templates
"يجب [moveOrCopy] بيانات [dataDescription] من [dataOrigin] إلى [dataDestination] بمعدل تكرار [frequency]."

### [6] Examples
* **مثال 1:** يجب نقل (Move) الطلبات التي مر على حالتها كـ (ملغي) أكثر من 90 يوماً من قاعدة بيانات الطلبات النشطة إلى قاعدة بيانات الأرشيف (Archive Database) ويتم ذلك شهرياً بشكل تلقائي (أو يدوي عبر الـ Owner).

### [7] Development & Testing Considerations
* **للمطور:** قد يتطلب ذلك كتابة `Cron Job` في بيئة Node.js يقوم بالبحث التلقائي في الداتا بيس عن الطلبات القديمة وترحيلها لتخفيف أحجام الجداول النشطة وتحسين الاستعلامات.
* **المختبر:** التحقق من أن الاستعلامات اليومية (Inquiries) لا تستعلم من قاعدة بيانات الأرشيف لكي لا يبطئ النظام.

---

## 19. Fee/Tax Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Fee/Tax |
| **Domain** | Commercial Domain |
| **Group** | Financial Group |
| **Anticipated Frequency** | من متطلب واحد إلى دزينة متطلبات. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتحديد أي رسوم مالية (رسوم شحن، عمولات، أو ضرائب) يجب أن يقوم النظام بحسابها أو الإبلاغ عنها أو إضافتها لفاتورة.
* **When NOT to use:** لا يُستخدم للقيم الأساسية (مثل سعر المنتج نفسه Base Price).

### [3] Discussion
مصاريف الشحن (Shipping Fees) هي جزء حيوي في عملية الدفع داخل Fluffy. يجب أن تُحسب بدقة وتضاف إلى الإجمالي لتجنب أي خسائر للشركة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **feeName** | إلزامي (Mandatory) | اسم الرسوم (مثل: مصاريف الشحن Shipping Fee). |
| **basis** | إلزامي (Mandatory) | طبيعة حسابها (نسبة مئوية أم مبلغ ثابت). |
| **origin** | إلزامي (Mandatory) | على أي شيء تُطبق؟ (على الطلبات المرسلة למحافظات محددة). |
| **feeRateDeterminants**| إلزامي (Mandatory)| محددات الرسوم (اسم المحافظة المختارة). |

### [5] Templates
"يجب أن يقوم النظام بحساب [feeName] على أساس [basis] على كل [origin] بشرط [condition]. يتم الدفع من [payer] لـ [receiver] وقت [whenLevied]. وتُحدد قيمة الرسوم بناءً على [feeRateDeterminants]."

### [6] Examples
* **مثال 1:** يقوم النظام بحساب (مصاريف الشحن Shipping Fee) كـ (مبلغ ثابت Fixed Amount) على (طلب العميل) عند إتمامه. تُدفع من (العميل) لـ (الموقع). يتم تحديد المبلغ بناءً على (المحافظة Governorate) المختارة في صفحة `Cart.tsx` والمُسجلة في إعدادات الشحن `Shipping Rates`.

### [7] Development & Testing Considerations
* **للمطور:** يجب فصل `ShippingFee` في الداتا بيس داخل حقل مستقل في سجل الطلب وليس دمجها مع أسعار المنتجات لضمان دقة التقارير المالية.
* **المختبر:** اختبار اختيار محافظات مختلفة في واجهة عربة التسوق للتأكد من تغير قيمة الشحن ديناميكياً وإضافتها بشكل صحيح لمجموع السلة.

---

## 20. Inter-System Interface Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inter-System Interface |
| **Domain** | Fundamental Domain |
| **Group** | Inter-system Interfaces |
| **Anticipated Frequency** | من متطلبين إلى عدد كبير حسب تعقيد وارتباط النظام (Integrations). |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتحديد التفاعل وقنوات الاتصال بين النظام الذي نبنيه وأنظمة وبرمجيات خارجية (مثل بوابات الدفع، أتمتة الـ Webhooks، أو واجهات الذكاء الاصطناعي).
* **When NOT to use:** لا يُستخدم لواجهات المستخدم (User Interfaces) أو التواصل الداخلي بين مكونات نفس النظام.

### [3] Discussion
الأنظمة لم تعد تعمل كجزر منعزلة. في Fluffy، النظام يعتمد بشكل أساسي على التكامل مع `n8n` لمعالجة الطلبات، ومع نماذج `Hugging Face / Kolors` الخارجي لتوفير ميزة القياس الافتراضي (VTO).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | اسم الواجهة (مثل: n8n Webhook Interface). |
| **components** | إلزامي (Mandatory) | الأنظمة المتصلة (مثل: Fluffy Backend و n8n Platform). |
| **interfacePurpose**| إلزامي (Mandatory) | الغرض منها (مزامنة بيانات الطلبات). |
| **initiator** | إلزامي (Mandatory) | من يبدأ الاتصال (النظام الخاص بنا). |

### [5] Templates
"يجب أن تكون هناك واجهة اتصال محددة بوضوح تسمى [interfaceName] بين [component1] و [component2]. يتم تفعيلها بواسطة [initiator] لغرض [interfacePurpose]."

### [6] Examples
* **مثال 1:** توجد واجهة اتصال تسمى `n8n Webhook (i1)` بين نظام Fluffy وخدمة الأتمتة n8n. يتم إرسال طلب `POST` من سيرفر النظام لغرض إرسال بيانات الطلب أو طلبات المرتجعات لإكمال سلسلة المعالجة اللوجستية في n8n.
* **مثال 2:** توجد واجهة اتصال تسمى `HuggingFace VTO (i2)` بين النظام ونموذج `fashn-vton-1.5`. يتم تفعيلها من النظام لغرض معالجة صورتين (صورة العميل والمنتج) وإرجاع صورة مدمجة.

### [7] Development & Testing Considerations
* **للمطور:** يجب الحذر الشديد من فشل هذه الواجهات الخارجية بتطبيق سياسات `Timeout` و `Error Handling` سليمة عبر كتل `try/catch`، كما هو مطبق فعلياً في أكواد مشروع Fluffy لحماية السيرفر من التعطل في حال تأخر رد الـ AI.
* **المختبر:** اختبار السيناريو في حال تعطل النظام الخارجي للتأكد من أن واجهة الموقع تظهر رسالة خطأ محترمة وتستمر في العمل بدلاً من أن تتعطل (Crash).

---

## 21. Technology Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Technology |
| **Domain** | Fundamental Domain |
| **Group** | Technical Constraints Group |
| **Anticipated Frequency** | من 1 إلى 5 متطلبات. |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: Maybe |

### [2] Applicability
* **When to use:** يُستخدم عندما يكون هناك قيد أو قرار تقني إلزامي يجب أن يلتزم به فريق التطوير (مثل اختيار لغة برمجة، أو نوع قاعدة بيانات معين).
* **When NOT to use:** لا يُستخدم لتحديد معايير واجهات المستخدم أو الأداء، بل للتقنيات الأساسية (Tech Stack).

### [3] Discussion
القرارات التقنية في مشروع Fluffy محسومة وتستند إلى بنية MERN Stack الحديثة لتوفير أداء عالٍ (Non-blocking I/O) وسهولة في التعامل مع البيانات غير المهيكلة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **technologyType** | إلزامي (Mandatory) | نوع التقنية (مثل: Backend Server, Database, Frontend). |
| **technologyName** | إلزامي (Mandatory) | اسم التقنية أو لغة البرمجة (مثل: Node.js, MongoDB, React). |

### [5] Templates
"يجب أن يستخدم النظام تقنية [technologyName] لغرض بناء وتشغيل [technologyType]."

### [6] Examples
* **مثال 1:** يجب أن يستخدم النظام تقنية `Node.js` و `Express.js` لغرض بناء وتشغيل الخادم الخلفي (Backend Server).
* **مثال 2:** يجب أن يستخدم النظام قاعدة بيانات `MongoDB` (NoSQL) باستخدام مكتبة `Mongoose` لغرض حفظ وإدارة البيانات (Database).

### [7] Development & Testing Considerations
* **للمطور:** الالتزام بهذه التقنيات يعني استغلال ميزاتها (مثل استخدام `async/await` في Node.js واستخدام الـ `Schemas` في Mongoose لضبط البيانات).
* **المختبر:** التأكد من إعداد البيئة الصحيحة (Environment) وتوفر هذه التقنيات قبل بدء الـ Integration Testing.

---

## 22. Data Structure Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Structure |
| **Domain** | Information Domain |
| **Group** | Information Structure Group |
| **Anticipated Frequency** | من 5 إلى 20 متطلباً. |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتحديد تركيبة بيانية تتكون من عدة حقول مترابطة يتم استخدامها ككتلة واحدة في أكثر من مكان.
* **When NOT to use:** لا يُستخدم لوصف كيان كامل مستقل (يُستخدم Living Entity أو Transaction)، بل لأجزاء البيانات المتكررة (مثل هيكل عنوان الشحن).

### [3] Discussion
في Fluffy، تفاصيل عنوان العميل (Shipping Address) وتفاصيل السلة (Order Items) تعتبر هياكل بيانية تُمرر من الـ Frontend للـ Backend وتُحفظ كجزء من الطلب.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **structureName** | إلزامي (Mandatory) | اسم الهيكل (مثل: Order Item Details). |
| **itemsOfInformation**| إلزامي (Mandatory) | الحقول التي يتكون منها (مثل: productId, color, size, qty). |

### [5] Templates
"يجب أن تتكون التركيبة البيانية [structureName] من المعلومات التالية: [itemsOfInformation]."

### [6] Examples
* **مثال 1:** يجب أن تتكون التركيبة البيانية لـ `Order Item Details` من: معرف المنتج `productId`، اسم المنتج، اللون، المقاس، الكمية (كحد أدنى 1)، وسعر القطعة. يتم حفظ هذا الهيكل داخل مصفوفة `items` في جدول الطلبات.

### [7] Development & Testing Considerations
* **للمطور:** يجب تعريف هذه الهياكل كـ `Sub-documents` في `Mongoose`، والتأكد من التحقق من صحتها (Validation) في الـ Backend.
* **المختبر:** إرسال طلب شراء بدون حقل (المقاس) والتأكد أن السيرفر يقبله إن كان اختيارياً، أو يرفضه إن كان إلزامياً بحسب الـ Schema.

---

## 23. Living Entity Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Living Entity |
| **Domain** | Data Entity Domain |
| **Group** | Business Entities Group |
| **Anticipated Frequency** | متطلب واحد لكل كيان أساسي (مثل: Product, User, FactoryClient). |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لوصف الكيانات المادية أو المنطقية الدائمة في النظام والتي لها دورة حياة (تُضاف، تُعدل، وتُحذف) مثل المنتجات والعملاء.
* **When NOT to use:** لا يُستخدم لسجلات المعاملات والأحداث التي تحدث في نقطة زمنية ولا تتغير (يُستخدم Transaction Pattern).

### [3] Discussion
المنتج (Product) هو الكيان الحي الأهم في Fluffy، له خصائص تتغير بمرور الوقت (مثل المخزون والتقييمات)، بينما معرّفه يبقى ثابتاً.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | اسم الكيان (مثل: Product). |
| **informationToStore**| إلزامي (Mandatory) | قائمة الحقول والخصائص (Name, Price, Stock, Category...). |

### [5] Templates
"يجب أن يقوم النظام بحفظ المعلومات التالية عن كل [entityName] يتم تسجيله: [informationToStore]."

### [6] Examples
* **مثال 1:** يجب أن يقوم النظام بحفظ المعلومات التالية عن كل `Product`: الاسم التجاري، السعر، الوصف، الصور (مصفوفة روابط)، القسم `category`، المقاسات والألوان المتاحة، وكمية المخزون `stock` (بقيمة افتراضية 0)، بالإضافة لحقول إحصائية مثل التقييم ومرات البيع `soldCount`.

### [7] Development & Testing Considerations
* **للمطور:** يجب إنشاء `Mongoose Model` يمثل هذا الكيان لضمان فرض القيود وأنواع البيانات المذكورة في هذا المتطلب.
* **المختبر:** التحقق من قدرة النظام على تحديث إحدى الخصائص (مثل تقليل الـ `stock`) دون أن تتأثر باقي خصائص الكيان.

---

## 24. Transaction Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Transaction |
| **Domain** | Data Entity Domain |
| **Group** | Business Entities Group |
| **Anticipated Frequency** | متطلب واحد لكل نوع من أنواع المعاملات (مثل: Order, Payment). |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لوصف سجلات المعاملات والأحداث التي تقع في لحظة معينة (Point in time) وتكون غالباً غير قابلة للتعديل الجذري لاحقاً.
* **When NOT to use:** لا يُستخدم للكيانات الحية (Living Entities) التي تتغير تفاصيلها باستمرار.

### [3] Discussion
في Fluffy، إنشاء الطلب (Order) يُعتبر (Transaction). يتم تسجيل تفاصيل العملية وقيمة الفاتورة اللحظية ولحظة الإنشاء لضمان حقوق العميل والشركة، كما يتزامن ذلك مع تحديث مخزون المنتجات المباعة (Atomicity).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **transactionName** | إلزامي (Mandatory) | اسم المعاملة (مثل: Customer Order). |
| **informationToStore**| إلزامي (Mandatory) | تفاصيل المعاملة (Customer Info, Items array, Total Amount, Date). |

### [5] Templates
"يجب أن يقوم النظام بحفظ المعلومات التالية عن كل معاملة من نوع [transactionName]: [informationToStore]."

### [6] Examples
* **مثال 1:** يجب أن يقوم النظام بحفظ المعلومات التالية عن كل معاملة `Customer Order`: اسم العميل، هاتفه، عنوانه الكامل شامل المحافظة، مصاريف الشحن اللحظية، تفاصيل المشتريات (Order Items)، المبلغ الإجمالي للطلب، حالة الطلب (قيد الانتظار كقيمة افتراضية)، وتاريخ ووقت المعاملة.

### [7] Development & Testing Considerations
* **للمطور:** يجب أن تتم معاملة الطلب جنباً إلى جنب مع عملية خصم المخزون بشكل آمن (`$inc` في Mongoose) لمنع مشاكل الـ Race Conditions عند وجود ضغط شراء.
* **المختبر:** التحقق من أن تغيير سعر المنتج في المتجر لاحقاً لا يؤثر على قيمة `Total Amount` المسجلة في معاملة الطلب القديمة للعميل.

---

## 25. Configuration Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Configuration |
| **Domain** | Data Entity Domain |
| **Group** | Business Entities Group |
| **Anticipated Frequency** | من 1 إلى 5 متطلبات. |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لوصف الإعدادات التي تحكم قواعد العمل في النظام ويمكن لمدير النظام تغييرها متى شاء للتكيف مع تغيرات العمل.
* **When NOT to use:** لا يُستخدم لحفظ التفضيلات الشخصية للمستخدم العادي (User Preferences).

### [3] Discussion
أسعار الشحن (Shipping Rates) تختلف من محافظة لأخرى، والشركة قد تغير عقود شركة الشحن، لذا تم تخصيص جدول إعدادات `Settings` في قاعدة البيانات ليتيح للأدمن تعديل الأسعار ديناميكياً من الـ `OwnerDashboard`.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **configurationName**| إلزامي (Mandatory) | اسم الإعداد (مثل: Shipping Rates). |
| **informationToStore**| إلزامي (Mandatory) | البيانات المخزنة (مثل خريطة المحافظات والأسعار). |

### [5] Templates
"يجب أن يقوم النظام بحفظ إعدادات تكوين باسم [configurationName] والتي تتكون من: [informationToStore]."

### [6] Examples
* **مثال 1:** يجب أن يقوم النظام بحفظ إعدادات تكوين باسم `Shipping Rates` والتي تتكون من خريطة برمجية (Mapping) تربط بين اسم المحافظة (مثلاً: القاهرة) وتكلفة الشحن الخاصة بها بالدولار أو الجنيه.

### [7] Development & Testing Considerations
* **للمطور:** تم تنفيذ ذلك باستخدام موديل `Settings.js` بحقل `type: 'shipping'` وحقل `rates: Object`. يجب أن يقرأ سيرفر הطلبات `OrderController` من هذا الإعداد دائماً لضمان حساب التكلفة الصحيحة.
* **المختبر:** تغيير سعر الشحن لمحافظة (الإسكندرية) من لوحة التحكم، ثم عمل طلب جديد لنفس المحافظة للتأكد من انعكاس السعر الجديد فوراً في سلة العميل.

---

## 26. Response Time Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Response Time |
| **Domain** | Performance Domain |
| **Group** | Performance Constraints Group |
| **Anticipated Frequency** | من 2 إلى 10 متطلبات. |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Maybe |

### [2] Applicability
* **When to use:** يُستخدم لفرض حد أقصى للزمن الذي يستغرقه النظام في تنفيذ عملية معينة وإرجاع النتيجة للمستخدم (لضمان تجربة مستخدم سريعة).
* **When NOT to use:** لا يُستخدم لقياس سرعة نقل البيانات الكلية للشبكة.

### [3] Discussion
معالجة الصور بالذكاء الاصطناعي (VTO) عبر واجهة `Hugging Face` تستغرق وقتاً طويلاً بطبيعتها، لكن يجب ألا ينتظر العميل للأبد. في حالة نفاد رصيد الذكاء الاصطناعي (ZeroGPU)، يجب أن يرد النظام بخطأ أو برد وهمي (Mock) بسرعة لا تتجاوز ثانيتين، كما نصّ عليه ملف הـ `README` لديك.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **transactionOrFunction**| إلزامي (Mandatory)| العملية المستهدفة (VTO AI Processing). |
| **responseTime** | إلزامي (Mandatory)| الحد الأقصى للزمن (15 ثانية للمعالجة، أو 2 ثانية في الـ Fallback). |
| **conditions** | إلزامي (Mandatory)| الظروف (تحت الضغط العادي أو عند نفاد الـ Quota). |

### [5] Templates
"يجب أن يقوم النظام بتنفيذ [transactionOrFunction] في مدة لا تتجاوز [responseTime] في ظل ظروف [conditions]."

### [6] Examples
* **مثال 1:** يجب أن يقوم النظام بتنفيذ عملية (تجربة القياس الافتراضي VTO) في مدة لا تتجاوز (15 ثانية) في ظل (عمل واجهة Hugging Face بشكل طبيعي).
* **مثال 2:** يجب أن يقوم النظام بتنفيذ عملية (الرد الاحتياطي AI Fallback) في مدة لا تتجاوز (ثانيتين) في ظل (نفاد رصيد ZeroGPU) وذلك بإرجاع رد Mocked.

### [7] Development & Testing Considerations
* **للمطور:** يجب وضع مؤشرات بصرية في واجهة المستخدم (Loading Spinners) لتخفيف شعور العميل بالانتظار وتجنب النقرات المتكررة التي قد تغرق السيرفر.
* **المختبر:** قطع الاتصال بالإنترنت أثناء طلب المعالجة للتحقق من أن النظام ينهي الطلب ويعرض رسالة الخطأ دون أن يدخل العميل في انتظار لا نهائي (Infinite loop).

---

## 27. Throughput Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Throughput |
| **Domain** | Performance Domain |
| **Group** | Performance Constraints Group |
| **Anticipated Frequency** | من 1 إلى 5 متطلبات. |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Maybe |

### [2] Applicability
* **When to use:** يُستخدم لتحديد قدرة النظام على معالجة عدد محدد من الطلبات أو العمليات خلال فترة زمنية معينة للتعامل مع أوقات الذروة.
* **When NOT to use:** لا يُستخدم لقياس سرعة استجابة طلب فردي (يُستخدم Response Time).

### [3] Discussion
خلال فترات الخصومات الكبرى (Sales)، قد يستقبل النظام آلاف الزوار. يجب أن يتحمل الخادم ضغطاً عالياً. بناءً على قيود الأداء المذكورة، نود معالجة 100 طلب في الثانية.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **process** | إلزامي (Mandatory) | نوع المعالجة (استقبال ومعالجة طلبات API Backend). |
| **throughputRate** | إلزامي (Mandatory) | المعدل المطلوب (100 طلب متزامن في الثانية). |
| **conditions** | إلزامي (Mandatory) | الظروف (أوقات الذروة والتخفيضات). |

### [5] Templates
"يجب أن يكون النظام قادراً على معالجة [process] بمعدل لا يقل عن [throughputRate] في ظل ظروف [conditions]."

### [6] Examples
* **مثال 1:** يجب أن يكون النظام قادراً على معالجة `استقبال طلبات الخادم (Backend APIs)` بمعدل لا يقل عن `100 طلب متزامن في الثانية` في ظل `فترات الذروة العالية`، معتمداً على بنية Node.js Non-blocking I/O.

### [7] Development & Testing Considerations
* **للمطور:** قد تحتاج إلى استخدام أدوات تجميع الموارد (Clustering) أو `PM2` لتشغيل أكثر من نسخة من سيرفر Node.js لاستغلال تعدد أنوية المعالج (CPU Cores).
* **المختبر:** استخدام أدوات مثل `JMeter` أو `Artillery` لمحاكاة 100 طلب في الثانية ومراقبة ما إذا كان السيرفر سيتحطم (Crash).

---

## 28. Dynamic Capacity Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Dynamic Capacity |
| **Domain** | Performance Domain |
| **Group** | Performance Constraints Group |
| **Anticipated Frequency** | من 1 إلى 5 متطلبات. |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتحديد عدد العناصر المتحركة أو النشطة في النظام في وقت واحد (مثل المستخدمين المتصلين، الجلسات، أو استخدام الذاكرة).
* **When NOT to use:** لا يُستخدم لحجم البيانات المخزنة في قاعدة البيانات (يُستخدم Static Capacity).

### [3] Discussion
النظام لديه قيود على استهلاك الذاكرة العشوائية (RAM) لضمان عدم توقف الخدمة. حددنا أن استهلاك الخادم للذاكرة يجب أن يظل منخفضاً وألا يتجاوز حد 256 ميجابايت في وضع الخمول (Idle).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **subject** | إلزامي (Mandatory) | العنصر الحيوي المستهدف (مساحة استهلاك الـ RAM للخادم). |
| **capacity** | إلزامي (Mandatory) | الحد الأقصى أو الأدنى المطلوب (لا يتجاوز 256MB). |

### [5] Templates
"يجب أن يكون النظام قادراً على دعم وتحمل [capacity] من [subject]."

### [6] Examples
* **مثال 1:** يجب أن يكون النظام (تحديداً عملية الـ Backend) قادراً على دعم استهلاك موارد بحيث يكون أقصى استهلاك للذاكرة هو `256MB RAM` من (استهلاك الذاكرة في وضع الخمول Idle).
* **مثال 2:** يجب أن يكون النظام قادراً على دعم ما يصل إلى `100` من (العملاء المتصلين النشطين المتزامنين).

### [7] Development & Testing Considerations
* **للمطور:** تأكد من عدم تخزين الملفات والصور الكبيرة المرفوعة في ذاكرة الوصول العشوائي للخادم (Buffer Memory Leak) بل احفظها مؤقتاً في نظام الملفات المحلّي (Disk) أو قم بتوجيهها مباشرة لواجهة VTO الخارجية.
* **المختبر:** مراقبة استهلاك الـ RAM باستخدام `Task Manager` أو `Docker stats` أثناء الضغط على النظام للتأكد من عدم حدوث تسرّب للذاكرة (Memory Leak).

---

## 29. Static Capacity Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Static Capacity |
| **Domain** | Performance Domain |
| **Group** | Performance Constraints Group |
| **Anticipated Frequency** | من 1 إلى 5 متطلبات. |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتحديد حجم السعة التخزينية القصوى لقاعدة البيانات أو كمية السجلات التي يجب أن تكون قادرة على تخزينها قبل الحاجة للتوسعة.
* **When NOT to use:** لا يُستخدم لقياس النشاط المتزامن أو المرور السريع للبيانات.

### [3] Discussion
المنصة ستجمع آلاف السجلات مع مرور الوقت (عملاء، طلبات، عمال، وسجلات الديون). استخدام خطة متدرجة في التخزين (MongoDB Atlas Tiered Storage) هو أمر بالغ الأهمية.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **subject** | إلزامي (Mandatory) | الكيان المراد تخزينه (مثل: Customer Orders و Images). |
| **capacity** | إلزامي (Mandatory) | الكمية القصوى المتوقعة (مثل: ملايين السجلات). |

### [5] Templates
"يجب أن يكون النظام قادراً على تخزين ما لا يقل عن [capacity] من [subject]."

### [6] Examples
* **مثال 1:** يجب أن يكون النظام قادراً على تخزين ما لا يقل عن `200,000 سجل` من (طلبات العملاء Customer Orders) مع الحفاظ على سرعة الاستعلام باستخدام الفهارس (Indexes).

### [7] Development & Testing Considerations
* **للمطور:** الصور (Images) لا تُخزن كملفات `Base64` داخل قاعدة بيانات MongoDB لتوفير السعة، بل يُفضل تخزين الروابط النصية (URLs) لموفر خدمة سحابي مثل AWS S3 أو Cloudinary، كما فعلنا في الكود.
* **المختبر:** مراجعة حجم قاعدة البيانات الحالية والتأكد من أنها لا تتضخم بشكل غير طبيعي بسبب التخزين الخاطئ.

---

## 30. Availability Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Availability |
| **Domain** | Performance Domain |
| **Group** | Performance Constraints Group |
| **Anticipated Frequency** | من 1 إلى 3 متطلبات. |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتحديد مستوى توفر وعمل النظام المطلق (Uptime) للمستخدمين، وتحديد نسب الأعطال المقبولة خلال السنة.
* **When NOT to use:** لا يُستخدم كضمان لسرعة الاستجابة، بل يعنى بوجود الخدمة أو سقوطها بالكامل.

### [3] Discussion
أي توقف في متجر Fluffy يعني خسارة في المبيعات وفقدان ثقة المصانع. كما نصت قيود الأداء، هدف التوفر لدينا هو 99.5% للوظائف التجارية الأساسية (Endpoints).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **systemOrComponent**| إلزامي (Mandatory) | النظام أو المكون (Core Commerce Endpoints). |
| **availabilityTarget** | إلزامي (Mandatory) | نسبة أو وقت التوفر (99.5%). |

### [5] Templates
"يجب أن يكون [systemOrComponent] متاحاً للاستخدام نسبة [availabilityTarget] من الوقت على الأقل."

### [6] Examples
* **مثال 1:** يجب أن تكون (واجهات التجارة الأساسية Core Commerce Endpoints - كإنشاء الطلبات وعرض المنتجات) متاحة للاستخدام نسبة `99.5%` من الوقت على الأقل (ما يعادل توقف لا يزيد عن 3.6 ساعة شهرياً).

### [7] Development & Testing Considerations
* **للمطور:** لتحقيق هذه النسبة العالية، يجب استضافة السيرفر وقاعدة البيانات على خدمات سحابية موثوقة (Cloud Providers) توفر ميزة الـ Auto-restart والـ Failover.
* **المختبر:** التحقق من وجود خدمة مراقبة خارجية (مثل UptimeRobot) ترسل تنبيهات لمدير النظام فور سقوط السيرفر لسرعة التدخل.

---

## 31. User Interface Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | من 5 إلى 20 متطلباً. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتحديد شاشات التفاعل الأساسية التي سيستخدمها المستخدم لإدخال البيانات واستعراضها، والتي تمثل نقطة تماس حقيقية مع العميل.
* **When NOT to use:** لا يُستخدم لواجهات النظام البرمجية الخلفية (APIs) أو الـ Webhooks (تلك وظيفة نمط Inter-System Interface).

### [3] Discussion
شاشة الدفع وعربة التسوق (Checkout & Cart) هي الواجهة الأهم لمتجر Fluffy، حيث يعتمد عليها معدل التحويل المالي للمشروع بالكامل، ويجب أن تكون بديهية لجمع بيانات الشحن.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | اسم الواجهة (مثل: Checkout User Interface). |
| **interfacePurpose**| إلزامي (Mandatory) | الغرض منها (جمع بيانات الشحن وتأكيد الطلب). |
| **users** | إلزامي (Mandatory) | المستخدمون المستهدفون (Customers). |
| **informationInput**| إلزامي (Mandatory) | المدخلات (الاسم، العنوان، التليفون، المحافظة). |
| **informationOutput**| إلزامي (Mandatory)| المخرجات (الفاتورة النهائية، المنتجات، الإجمالي). |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة باسم `Checkout Interface` بغرض تأكيد الطلبات المشتراة. متاحة للعملاء، تقبل إدخال: الاسم، رقم الهاتف، العنوان، واختيار المحافظة. وتعرض: قائمة مشتريات السلة والتكلفة الإجمالية بعد الشحن.

### [7] Development & Testing Considerations
* **للمطور:** تم بناء هذه الواجهة باستخدام `React` ومكونات `Tailwind CSS`. يجب التأكد من عمل (Form Validation) لمنع العميل من الضغط على "تأكيد" دون إدخال حقل "المحافظة".
* **المختبر:** فتح الواجهة من متصفح هاتف محمول (Mobile View) للتأكد من استجابتها (Responsive Design) وعدم تداخل العناصر.

---

## 32. Accessibility Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Accessibility |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | من 1 إلى 5 متطلبات. |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتحديد المتطلبات التي تضمن أن الأشخاص ذوي الإعاقة أو التحديات (مثل عمى الألوان) يمكنهم استخدام النظام بكفاءة.
* **When NOT to use:** لا يُستخدم كمتطلب لسهولة الاستخدام العادية للأشخاص الأصحاء (Usability).

### [3] Discussion
متجر Fluffy يعتمد بشدة على الألوان (ألوان الفساتين والملابس). إذا كان المستخدم يعاني من "عمى الألوان"، فلن يتمكن من التمييز عبر المربعات اللونية فقط.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **disabilityType** | إلزامي (Mandatory) | نوع التحدي (عمى الألوان Color Blindness). |
| **extentOfAccessibility**| إلزامي (Mandatory)| مدى المعالجة (عدم الاعتماد على اللون كوسيلة وحيدة). |
| **standardToComplyWith** | اختياري (Optional) | المعيار (مثل: WCAG 2.1). |

### [5] Templates
"يجب أن يكون النظام قابلاً للوصول والاستخدام من قبل الأشخاص الذين يعانون من [disabilityType] إلى حد [extentOfAccessibility]. يجب أن يتوافق ذلك مع معيار [standardToComplyWith]."

### [6] Examples
* **مثال 1:** يجب أن يكون النظام قابلاً للاستخدام من قبل الأشخاص الذين يعانون من (عمى الألوان) إلى حد (أن الترميز اللوني لن يكون الوسيلة الوحيدة لنقل المعلومات)، عبر توفير "الاسم النصي للون" (مثل: أحمر، كحلي) بجوار أو عند تمرير الماوس فوق المربع اللوني.

### [7] Development & Testing Considerations
* **للمطور:** تمت مراعاة ذلك في كود `ProductDetail.tsx` بإضافة خاصية `title={color}` إلى زر اختيار اللون لكي تظهر كنص توضيحي.
* **المختبر:** التحقق من جميع مؤشرات الحالة (مثل "متوفر" أو "غير متوفر") وأنها لا تعتمد فقط على الألوان (أخضر/أحمر) بل تحتوي على نص صريح واضح.

---

## 33. Scalability Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Scalability |
| **Domain** | Flexibility Domain |
| **Group** | Flexibility Group |
| **Anticipated Frequency** | من 1 إلى 4 متطلبات. |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: Maybe |

### [2] Applicability
* **When to use:** يُستخدم لضمان أن النظام صُمم هندسياً بحيث يمكنه استيعاب نمو مستقبلي ضخم دون الحاجة لإعادة كتابة الكود البرمجي من الصفر.
* **When NOT to use:** لا يُستخدم لتحديد السعة التخزينية المادية اللحظية (تلك وظيفة Static Capacity).

### [3] Discussion
الأتيليه قد يبدأ بـ 50 منتجاً، ولكن بعد سنة قد يصل لـ 10,000 منتج. واجهة المستخدم وقاعدة البيانات يجب ألا تنهار أو تصبح بطيئة مع هذا التوسع السريع.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **scalingTarget** | إلزامي (Mandatory) | الهدف الاستيعابي المستقبلي (عشرات الآلاف). |
| **subjectOfScaling**| إلزامي (Mandatory) | الموضوع المراد توسعه (عدد المنتجات Product Catalog). |
| **motivation** | إلزامي (Mandatory) | الدافع (استيعاب خطوط الإنتاج الجديدة للمصنع). |

### [5] Templates
"يجب أن يكون النظام قابلاً للتوسع (Scalable) لاستيعاب [scalingTarget] من [subjectOfScaling]. الدافع وراء هذا التطلب هو [motivation]."

### [6] Examples
* **مثال 1:** يجب أن يكون النظام قابلاً للتوسع لاستيعاب `عدد غير مقيد (يصل لعشرات الآلاف)` من `المنتجات في المتجر`. الدافع وراء هذا هو `استيعاب التوسعات المستقبلية وإضافة أقسام ملابس جديدة دون الحاجة لإعادة هندسة التطبيق`.

### [7] Development & Testing Considerations
* **للمطور:** لتلبية هذا المتطلب مستقبلاً، يجب ألا تقوم واجهة الـ Frontend بجلب (Fetch) جميع المنتجات دفعة واحدة، بل يجب الاستعداد لتطبيق الـ `Pagination` أو `Infinite Scroll` في הـ API الخاص بمسار `/products`.
* **المختبر:** إضافة 1000 منتج وهمي في بيئة الاختبار (Load Testing) والتحقق من أن استعلام جلب المنتجات لا يتسبب في تجميد السيرفر.

---

## 34. Extendability Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Extendability |
| **Domain** | Flexibility Domain |
| **Group** | Flexibility Group |
| **Anticipated Frequency** | من 1 إلى 5 متطلبات. |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: Maybe |

### [2] Applicability
* **When to use:** يُستخدم لضمان أن النظام مبني بطريقة معيارية (Modular) تسمح بـ "تركيب" ميزات أو برمجيات جديدة مستقبلاً دون إحداث تخريب في النواة الأساسية.
* **When NOT to use:** لا يُستخدم لعمليات الصيانة العادية أو إصلاح الأخطاء (Bug Fixes).

### [3] Discussion
حاليًا متجر Fluffy يعتمد على الدفع عند الاستلام (Cash on Delivery) ويحسب الإجمالي مباشرة. ولكن في المستقبل القريب، سيطلب المالك دمج بوابة دفع إلكتروني (مثل Paymob أو Stripe).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **extensionType** | إلزامي (Mandatory) | نوع التوسعة المبرمجة (Online Payment Gateways). |
| **easeOfExtension** | إلزامي (Mandatory) | سهولة التوسعة (بطريقة الـ Plug-and-play). |

### [5] Templates
"يجب أن يكون من الممكن توسيع النظام مستقبلاً عن طريق إضافة [extensionType]. إدخال مثل هذه الوحدات البرمجية يجب أن [easeOfExtension]."

### [6] Examples
* **مثال 1:** يجب أن يكون من الممكن توسيع النظام مستقبلاً عن طريق إضافة `وحدات لمعالجة الدفع الإلكتروني (Payment Gateways)`. إدخال هذه الوحدات يجب أن `يتم دون الحاجة لتغيير جذري في نواة قاعدة البيانات الأساسية، بحيث يتم توصيلها بسهولة بملف orderController`.

### [7] Development & Testing Considerations
* **للمطور:** كتابة الكود الخاص بـ الطلبات `Orders` بطريقة مفصولة ونظيفة، بحيث يكون تمرير حالة الدفع (Payment Status) كمتغير مرن يقبل الإضافة مستقبلاً دون تكسير الـ `OrderSchema`.
* **المختبر:** التحقق من أن بناء النظام يسمح بإنشاء طلب وتحديث حالته لاحقاً عبر واجهة منفصلة بمرونة تامة.

---

## 35. Multi-Lingual Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Multi-Lingual |
| **Domain** | Flexibility Domain |
| **Group** | Flexibility Group |
| **Anticipated Frequency** | متطلب واحد (إن وُجد). |
| **Classifications** | Functional: Maybe; Pervasive: Yes; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم عندما يكون من المتوقع أن يدعم النظام عرض واجهاته أو تخزين بياناته بأكثر من لغة واحدة استجابة لقاعدة عملاء عالمية.
* **When NOT to use:** لا يُستخدم إذا كان النظام محلياً ومغلقاً 100% للغة واحدة للأبد.

### [3] Discussion
الكود الحالي للواجهة الأمامية في Fluffy موجه للجمهور العربي (RTL)، ولكن الكود مكتوب بمرونة والمسميات باللغة الإنجليزية في قواعد البيانات. المتطلب ينص على ترك الباب مفتوحاً لإضافة الإنجليزية مستقبلاً.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **languages** | إلزامي (Mandatory) | اللغات المستهدفة (العربية والإنجليزية). |

### [5] Templates
"يجب أن يوفر النظام بنية تحتية تتيح مستقبلاً دعم واجهات متعددة اللغات، وتحديداً اللغات: [languages]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام بنية تحتية تتيح مستقبلاً دعم واجهات متعددة اللغات، وتحديداً: `اللغة العربية (RTL) واللغة الإنجليزية (LTR)`.

### [7] Development & Testing Considerations
* **للمطور:** تجنب "تشفير" (Hardcoding) النصوص المكتوبة باللغة العربية داخل ملفات `React` المعقدة، ويُفضل التخطيط لاستخدام مكتبات مثل `i18next` مستقبلاً لفصل النصوص عن الهيكل (UI).
* **المختبر:** التأكد من أن تغيير الـ `dir="rtl"` في الواجهة لا يتسبب في تحطم الـ Layout (خاصة في الـ Modals).

---

## 36. Installability Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Installability |
| **Domain** | Flexibility Domain |
| **Group** | Flexibility Group |
| **Anticipated Frequency** | متطلب واحد (للنظام الرئيسي). |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتحديد السهولة والأدوات المطلوبة لتثبيت التطبيق على خوادم الإنتاج أو بيئة تطوير جديدة.
* **When NOT to use:** لا يُستخدم لتحديثات النظام الطفيفة والمستمرة.

### [3] Discussion
نظام يتكون من MERN Stack (Node, Express, React) يتطلب إعدادات بيئية متعددة ومفاتيح سرية (API Keys). يجب أن يكون من السهل على مسؤول السيرفر تشغيله باستخدام أمر موحد وبسيط (مثل الـ npm).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **softwareComponent**| إلزامي (Mandatory) | المكون (نظام الـ Backend و الـ Frontend). |
| **installer** | إلزامي (Mandatory) | من يقوم بالتثبيت (System Administrator). |
| **easeOfInstallation**| إلزامي (Mandatory)| معيار السهولة (عبر سكريبت موحد أو Docker). |

### [5] Templates
"يجب أن يكون من الممكن تثبيت [softwareComponent] بواسطة [installer]. عملية التثبيت يجب أن تكون [easeOfInstallation]."

### [6] Examples
* **مثال 1:** يجب أن يكون من الممكن تثبيت `برمجيات النظام الأساسي (Backend & Frontend)` بواسطة `مسؤول نظام معتمد`. عملية التثبيت يجب أن تكون `مريحة وتتطلب إدخال معلومات قليلة عبر ملفات البيئة .env وتنفيذ أمر npm run start`.

### [7] Development & Testing Considerations
* **للمطور:** ضرورة توفير ملف `.env.example` يوضح للمشغل ما هي المفاتيح المطلوبة (مثل `JWT_SECRET`, `EMAIL_APP_PASSWORD`, `HUGGINGFACE_API_KEY`) لضمان نجاح التثبيت دون أخطاء.
* **المختبر:** استنساخ المشروع (Clone) في جهاز نظيف تماماً ومحاولة تشغيله للتأكد من عدم وجود مسارات ملفات معتمدة على جهاز المطور القديم (Hardcoded paths).

---

## 37. Configurable Authorization Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Configurable Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Configuration Group |
| **Anticipated Frequency** | من 1 إلى 3 متطلبات. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم عندما لا تكون صلاحيات النظام جامدة، بل يمكن للمدير الأعلى (Owner) تعيين الأدوار (Roles) وتغيير صلاحيات المستخدمين بحرية من داخل لوحة التحكم.
* **When NOT to use:** لا يُستخدم للقيود البرمجية الثابتة التي تحمي المسارات تقنياً (تلك Specific Authorization).

### [3] Discussion
النظام لديه أدوار متعددة: عميل، عامل، أدمن، مالك. في Fluffy، يمكن للأدمن تحويل عميل عادي إلى "أدمن" لإدارة الأتيليه.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **typesOfUsers** | إلزامي (Mandatory) | أنواع المستخدمين (الموظفون والإدارة). |
| **systemFunctions** | إلزامي (Mandatory) | النظام أو البيانات المستهدفة (الوظائف الداخلية). |

### [5] Templates
"يجب أن يسمح النظام لمستخدم مخول بضبط حقوق الوصول وصلاحيات [typesOfUsers] الخاصة بـ [systemFunctions]."

### [6] Examples
* **مثال 1:** يجب أن يسمح النظام لمستخدم مخول (بصلاحية Owner) بضبط حقوق الوصول وصلاحيات `الموظفين` الخاصة بـ `الوظائف الداخلية للمتجر (مثل إضافة المنتجات وتعديل أسعارها)` وذلك من خلال تعديل حقل الـ `role` في قاعدة بيانات المستخدمين.

### [7] Development & Testing Considerations
* **للمطور:** يجب توفير واجهة في الـ Frontend للأونر فقط، تتيح له استعراض المستخدمين وتغيير الـ `role` الخاص بهم بأمان.
* **المختبر:** التأكد من أن المستخدم الذي يحمل صلاحية `admin` لا يمكنه سحب صلاحيات مستخدم يحمل صلاحية أعلى منه مثل `owner`.

---

## 38. Approval Pattern

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Approval |
| **Domain** | Access Control Domain |
| **Group** | Access Control Configuration Group |
| **Anticipated Frequency** | من 1 إلى 5 متطلبات. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لفرض قاعدة أعمال (Business Rule) تتطلب من شخص ذو سلطة الموافقة على معاملة أو تسعيرة معينة قبل المضي قدماً في تنفيذها.
* **When NOT to use:** لا يُستخدم في عمليات التحقق الآلية العادية المبرمجة مسبقاً (مثل تحقق رصيد السلة).

### [3] Discussion
أهم منطقة حساسة في Fluffy هي طلبات الجملة (Wholesale Orders). المصنع الشريك يرفع الطلب والمواصفات للقطعة، لكن السعر الإجمالي (Total Price) لا يُحسب آلياً. بل تظل الحالة "في انتظار التسعير" حتى يضع الـ `Owner` سعر القطعة (`pricePerPiece`).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **actionRequiringApproval**| إلزامي (Mandatory) | الفعل الذي يطلب موافقة (تسعير طلبات الجملة). |
| **approverRole** | إلزامي (Mandatory) | دور المُوافق (Owner). |
| **rejectionAction** | إلزامي (Mandatory) | الإجراء عند الرفض أو التأخير (بقاء الطلب معلقاً). |

### [5] Templates
"يجب أن تكون عملية [actionRequiringApproval] معتمدة وموافق عليها دائماً من قبل [approverRole]. وفي حال عدم الموافقة أو التأخير، يجب أن يتم [rejectionAction]."

### [6] Examples
* **مثال 1:** يجب أن تكون عملية `تسعير والموافقة على بدء تشغيل طلبات الجملة B2B` معتمدة وموافق عليها دائماً من قبل الإدارة العليا `Owner أو Admin`. وفي حال عدم الموافقة أو تسعيرها، يجب أن يتم `إبقاء حالة الطلب كـ (في انتظار التسعير) وعدم إدراجه في خط الإنتاج`.

### [7] Development & Testing Considerations
* **للمطور:** في مسار التعديل `PUT /wholesale-orders/:id`، تم برمجة الكود بحيث إذا قام الـ Owner بإدخال رقم في `pricePerPiece`، تتحول الحالة آلياً من "في انتظار التسعير" إلى "قيد الانتظار" لتبدأ الموافقة والتشغيل.
* **المختبر:** التحقق من أن العميل التجاري (Factory Client) لا يستطيع بأي شكل من الأشكال إدخال سعر القطعة بنفسه من لوحة تحكمه لمنع التلاعب.

---

## 39. Report Pattern (Worker Payroll Instantiation)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Report (Worker Payroll) |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability & Discussion
قمنا بشرح نمط التقرير سابقاً، ولكننا هنا نطبق (Instantiation) جديد للنمط على كيان العمال (Workers). هذا التقرير يعتبر وثيقة مالية أساسية للورشة في نهاية كل شهر لجدولة المرتبات وصرفها.

### [3] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **reportName** | إلزامي (Mandatory) | Worker Payroll Report. |
| **businessIntent** | إلزامي (Mandatory) | استعراض ومراجعة المستحقات المالية الصافية للعمال. |
| **informationToShow** | إلزامي (Mandatory) | اسم العامل، الراتب الأساسي، الخصومات، والراتب الصافي. |
| **totalingLevels** | اختياري (Optional) | إجمالي المصروفات على الرواتب للورشة. |

### [4] Templates & Examples
"يجب أن يوفر النظام تقريراً باسم `Worker Payroll` لعرض `اسم العامل، الراتب، الخصومات، وأيام الحضور والغياب`. الغرض من التقرير هو `تسهيل صرف الرواتب الشهرية من قبل الإدارة`. يجب أن يتضمن التقرير إجماليات لـ `إجمالي الرواتب الصافية المستحقة الدفع لجميع العمال`."

### [5] Development & Testing Considerations
* **للمطور:** يجب الاعتماد على المعادلة التي أدرجناها في نمط (Calculation Formula) لحساب الراتب الصافي وقت توليد التقرير ديناميكياً.

---

## 40. Calculation Formula Pattern (Total Debt Instantiation)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula (Factory Debt) |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability & Discussion
هذا الاستنساخ (Instantiation) الثاني لنمط المعادلات الرياضية يُطبق على عمليات سداد ديون المصانع الشريكة في Fluffy، وهو يضمن أن الدورة المالية تعمل بدقة بين إجمالي الأوردرات المسلمة وبين الدفعات الكاش (Payments).

### [3] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | Remaining Debt (المتبقي من الديون). |
| **formula** | إلزامي (Mandatory) | Remaining Debt = TotalDebt - PaidAmount. |
| **variables** | إلزامي (Mandatory) | إجمالي الديون، إجمالي المدفوعات. |

### [4] Templates & Examples
"يجب حساب `Remaining Debt (المتبقي من المديونية للمصنع الشريك)` باستخدام المعادلة التالية: `Remaining Debt = Total Debt - Paid Amount`، حيث تمثل المتغيرات `مجموع أسعار الأوردرات المُسلمة للمصنع` و `مجموع المبالغ التي دفعها المصنع للإدارة`."

### [5] Development & Testing Considerations
* **للمطور:** الكود في `FactoryDashboard.tsx` يحسبها ديناميكياً كالتالي: `const remainingDebt = client.totalDebt - client.paidAmount;`. وتمت إزالة شرط (Math.max) للسماح بعرض مبالغ سالبة مما يعني أن الكيان له (رصيد دائن) لدى المصنع، وهذا يُعد دقة هندسية ومالية عالية جداً.

---

## 41. Data Maintenance Pattern (Order Cancellation & Restore Stock)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | متكرر (لكل كيان حيوي). |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتحديد الوظائف التي تسمح للمستخدم بتحديث حالة كيان معين أو إجراء صيانة لبياناته (تحديث أو حذف).
* **When NOT to use:** لا يُستخدم لعمليات الاستعلام والقراءة فقط (Inquiry).

### [3] Discussion
في مشروع Fluffy، إدارة المخزون حساسة جداً. إذا تم إلغاء الطلب، يجب ألا تضيع الكميات المحجوزة. الكود في مسار `/orders/restore-stock` يقوم بتحديث حالة الطلب إلى "ملغي" ويقوم بإرجاع الكميات للمنتجات في نفس اللحظة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Customer Order & Product Stock. |
| **functions** | إلزامي (Mandatory) | تحديث (Update) حالة الطلب لملغي وإرجاع المخزون. |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة لـ `تحديث حالة الطلب إلى (ملغي) وإرجاع الكميات (Stock) للمنتجات المرتبطة` لـ `Customer Order`.

### [7] Development & Testing Considerations
* **للمطور:** يجب استخدام عملية الزيادة `$inc` بقيمة موجبة في MongoDB للمنتج المرتبط لضمان إرجاع المخزون بشكل آمن دون التسبب في Race Conditions.
* **المختبر:** إنشاء طلب جديد وملاحظة نقص المخزون، ثم إلغاء الطلب والتأكد من عودة المخزون لرقمه الأصلي بدقة.

---

## 42. Data Maintenance Pattern (Worker Attendance Tracking)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | متكرر. |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** يُستخدم لتحديد وظيفة صيانة وتحديث بيانات الكيانات بشكل دوري أو يومي.

### [3] Discussion
متابعة العمال تتم يومياً في `OwnerDashboard.tsx`. يمكن للمدير بضغطة زر واحدة (زر + أو -) تحديث سجل حضور العامل أو غيابه لتسهيل الحسابات آخر الشهر.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Worker Profile. |
| **functions** | إلزامي (Mandatory) | تحديث (Update) أيام الحضور والغياب. |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة لـ `تحديث أيام الحضور (presentDays) وأيام الغياب (absentDays) بزيادة أو نقصان` لـ `Worker Profile`.

### [7] Development & Testing Considerations
* **للمطور:** يجب التأكد برمجياً من أن الدالة `updateWorkerStat` لا تسمح للقيم بأن تصبح بالسالب (باستخدام `Math.max(0, value)`).

---

## 43. Inter-System Interface Pattern (Email Notifications)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inter-System Interface |
| **Domain** | Fundamental Domain |
| **Group** | Inter-system Interfaces |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لتعريف ربط النظام بخدمة خارجية لإرسال البيانات دون انتظار معالجة معقدة (مثل إرسال إيميلات للعملاء).

### [3] Discussion
الاستنساخ الثاني لهذا النمط في Fluffy يركز على خدمة البريد الإلكتروني. النظام يتصل بـ `Gmail SMTP` عبر مكتبة `Nodemailer` لإرسال تأكيدات الطلبات وروابط إعادة تعيين كلمة المرور بشكل آلي.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | Email Service Interface (i3). |
| **components** | إلزامي (Mandatory) | Fluffy Backend و Gmail SMTP Server. |
| **interfacePurpose**| إلزامي (Mandatory) | إرسال رسائل تأكيد الطلبات وروابط إعادة التعيين. |
| **initiator** | إلزامي (Mandatory) | النظام الخاص بنا (Fluffy Backend). |

### [5] Templates
"يجب أن تكون هناك واجهة اتصال محددة بوضوح تسمى [interfaceName] بين [component1] و [component2]. يتم تفعيلها بواسطة [initiator] لغرض [interfacePurpose]."

### [6] Examples
* **مثال 1:** يجب أن تكون هناك واجهة اتصال محددة بوضوح تسمى `Email Service (i3)` بين `النظام` و `مزود خدمة البريد الإلكتروني (Gmail)`. يتم تفعيلها بواسطة `النظام` لغرض `إرسال رسائل HTML منسقة للعميل تحتوي على تفاصيل فاتورته فور إتمام الطلب`.

### [7] Development & Testing Considerations
* **للمطور:** الإرسال يجب أن يتم في الخلفية (Background process) لكي لا يتأخر رد السيرفر على العميل بـ 201 Created في حال كان سيرفر الإيميلات بطيئاً.

---

## 44. Inquiry Pattern (Dashboard Statistics)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inquiry |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لاستعراض إحصائيات سريعة للبيانات المجمعة دون السماح بتعديلها.

### [3] Discussion
شريط الإحصائيات العلوي في `OwnerDashboard.tsx` يعرض للمالك نظرة عامة لحظية على أداء المتجر.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **inquiryName** | إلزامي (Mandatory) | Dashboard Global Statistics. |
| **businessIntent** | إلزامي (Mandatory) | توفير رؤية سريعة لحجم العمل اللحظي للإدارة. |
| **informationToShow** | إلزامي (Mandatory) | إجمالي الإيرادات، عدد الطلبات، عدد العمال، إجمالي الديون. |

### [5] Templates
"يجب أن يوفر النظام استعلاماً باسم [inquiryName] لعرض [informationToShow]. الغرض منه هو [businessIntent]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام استعلاماً باسم `Dashboard Global Statistics` لعرض `مجموع الإيرادات، عدد الطلبات الكلي، عدد المنتجات المعروضة، ومجموع ديون المصانع الشريكة`.

### [7] Development & Testing Considerations
* **للمطور:** هذا الاستعلام يقوم بعمل Reduce للمصفوفات في الـ Frontend، وهو مناسب للأحجام المتوسطة. في حال زادت الداتا عن عشرات الآلاف، يفضل نقل هذا الاستعلام للـ Backend وعمل `MongoDB Aggregate` لتحسين الأداء.

---

## 45. Inquiry Pattern (Factory Client Orders History)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inquiry |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** يُستخدم لعرض سجلات تخص كيان محدد مع تطبيق فلترة إجبارية.

### [3] Discussion
المصنع الشريك يجب ألا يرى سوى الأوردرات المكلف بتنفيذها. يتم تطبيق هذا الاستعلام في `FactoryDashboard.tsx`.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **inquiryName** | إلزامي (Mandatory) | Factory Wholesale Orders History. |
| **businessIntent** | إلزامي (Mandatory) | مراجعة الطلبات المسندة للمصنع لمعرفة حالتها وأسعارها. |
| **informationToShow** | إلزامي (Mandatory) | اسم الموديل، الألوان، المقاسات، الكمية، السعر، والحالة. |
| **selectionCriteria** | اختياري (Optional) | التصفية إجبارياً بـ `clientId`. |

### [5] Templates
"يجب أن يوفر النظام استعلاماً باسم [inquiryName] لعرض [informationToShow]. الغرض منه هو [businessIntent]. يمكن تصفية البيانات باستخدام المعايير التالية: [selectionCriteria]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام استعلاماً باسم `Factory Wholesale Orders History` لعرض `سجل طلبات الجملة وتفاصيل الموديل والحالة الحالية`. يمكن تصفية البيانات باستخدام `معرّف العميل (clientId)` لضمان الخصوصية.

### [7] Development & Testing Considerations
* **المختبر:** تسجيل دخول بحسابين مختلفين لمصنعين، والتأكد من أن مصنع "أ" لا يمكنه نهائياً رؤية أوردرات مصنع "ب".

---

## 46. User Interface Pattern (Product Details Interface)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف واجهة التفاعل الدقيقة التي يبني العميل عليها قرار الشراء.

### [3] Discussion
شاشة `ProductDetail.tsx` هي من أعقد وأهم شاشات Fluffy، حيث تسمح بتحديد المقاسات والألوان الدقيقة لكل قطعة قبل الإضافة للسلة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | Product Detail Interface. |
| **interfacePurpose**| إلزامي (Mandatory) | عرض معلومات القطعة وتحديد المواصفات للشراء. |
| **users** | إلزامي (Mandatory) | عملاء الموقع (Customers/Visitors). |
| **informationInput**| إلزامي (Mandatory) | اختيار المقاس، اختيار اللون، وتحديد الكمية. |
| **informationOutput**| إلزامي (Mandatory) | الصور المتعددة للمنتج، السعر، الوصف، وحالة المخزون. |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة مستخدم باسم `Product Detail Interface` بغرض عرض تفاصيل المنتج وتسهيل الشراء. الواجهة متاحة للزوار، تعرض السعر والصور وحالة التوفر، وتقبل مدخلات: المقاس المطلوب، اللون المفضل، وتحديد كمية القطع.

### [7] Development & Testing Considerations
* **للمطور:** يجب برمجة Validation في الواجهة (Form) يمنع الضغط على "أضف للسلة" إذا لم يقم العميل باختيار اللون والمقاس في حال توفرهم للمنتج لتجنب إرسال طلبات ناقصة للباك-إند.

---

## 47. User Interface Pattern (Virtual Try-On UI)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف واجهة التفاعل لخدمة الذكاء الاصطناعي المدمجة بالمنصة.

### [3] Discussion
شاشة القياس الافتراضي (VTO Modal) تتطلب من المستخدم رفع صورته الخاصة ودمجها مع صورة الموديل، وهي واجهة تفاعلية لحظية.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | Virtual Try-On Modal Interface. |
| **interfacePurpose**| إلزامي (Mandatory) | قياس الملابس افتراضياً عبر الذكاء الاصطناعي. |
| **users** | إلزامي (Mandatory) | عملاء الموقع. |
| **informationInput**| إلزامي (Mandatory) | صورة شخصية مرفوعة (Human Image). |
| **informationOutput**| إلزامي (Mandatory) | صورة المعالجة النهائية (AI Generated Image). |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة باسم `Virtual Try-On Modal` بغرض تجربة قياس الملابس افتراضياً. تقبل المدخلات: إرفاق صورة شخصية عبر الجهاز. وتعرض المخرجات: الصورة المدمجة ومؤشرات تحميل (Loaders) أثناء المعالجة.

---

## 48. User Interface Pattern (Password Reset UI)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** واجهة مخصصة لمعالجة سيناريو نسيان كلمة المرور واسترجاعها عبر التوكن.

### [3] Discussion
إذا نسي العميل باسورده، النظام يرسل توكن (Token) عبر الإيميل. عند الضغط عليه، تفتح واجهة جديدة لتحديد كلمة المرور الجديدة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | Reset Password Interface. |
| **interfacePurpose**| إلزامي (Mandatory) | تمكين المستخدم من وضع كلمة مرور جديدة لحسابه. |
| **users** | إلزامي (Mandatory) | المستخدمون الذين يملكون Token صالح. |
| **informationInput**| إلزامي (Mandatory) | كلمة المرور الجديدة. |
| **informationOutput**| إلزامي (Mandatory) | رسائل النجاح أو فشل صلاحية الرابط. |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة باسم `Reset Password` لاستعادة الحساب. تقبل المدخلات: إدخال الباسوورد الجديد. وتعرض المخرجات: "تم تغيير كلمة المرور بنجاح" أو "انتهت صلاحية الرابط".

---

## 49. Specific Authorization Pattern (Factory Portal Access)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Specific Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ للنمط لتطبيق الحماية على واجهة المصانع التجارية (B2B) لمنع دخول العملاء الأفراد.

### [3] Discussion
العميل العادي (Customer) حسابه مسجل بـ Email، بينما المصنع مسجل بـ Username بجدول مختلف `FactoryClient`. يجب فصل البوابتين تماماً.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **userRole** | إلزامي (Mandatory) | نوع الجلسة (Session) أو التوكن. |
| **requiredRole** | إلزامي (Mandatory) | مسجل في جدول (FactoryClient). |
| **failureAction** | إلزامي (Mandatory) | الرفض وعرض واجهة الدخول الخاصة بالمصانع فقط. |

### [5] Templates
"يجب على النظام منع الوصول إلى [المسار / لوحة التحكم] وحجب البيانات تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق [الدور المصرح له]."

### [6] Examples
* **مثال 1:** يجب على النظام منع الوصول إلى `لوحة تحكم المصنع (Factory Dashboard)` وحجب بيانات الطلبيات تماماً إلا إذا كان المستخدم قد قام بتسجيل الدخول ببيانات معتمدة كـ `Factory Client`.

### [7] Development & Testing Considerations
* **للمطور:** تم تنفيذ ذلك باستخدام `localStorage` بمفتاح `factory_client` منفصل عن مفتاح دخول العميل العادي.

---

## 50. Specific Authorization Pattern (Inventory Management Access)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Specific Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** حماية مسارات الـ APIs الخلفية (Backend) المخصصة للتعديل والحذف.

### [3] Discussion
لا يكفي إخفاء الأزرار في الواجهة الأمامية، بل يجب حماية مسارات الخادم مثل `DELETE /products/:id` و `PUT /products/:id` لكي لا يتلاعب بها الهاكرز عبر برامج مثل Postman.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **userRole** | إلزامي (Mandatory) | دور المستخدم من الـ JWT. |
| **requiredRole** | إلزامي (Mandatory) | Owner أو Admin. |
| **failureAction** | إلزامي (Mandatory) | رفض الطلب برمز 401 Unauthorized أو 403 Forbidden. |

### [5] Templates
"يجب على النظام منع الوصول إلى [المسار / لوحة التحكم] وحجب البيانات تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق [الدور المصرح له]."

### [6] Examples
* **مثال 1:** يجب على النظام منع الوصول وتعديل البيانات عبر `مسارات الإضافة والتعديل والحذف (APIs الخاصة بالـ Products و Workers)` تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق `Owner أو Admin`.

### [7] Development & Testing Considerations
* **المختبر:** إرسال طلب `DELETE` على مسار المنتجات عبر Postman باستخدام `Token` الخاص بعميل عادي (Customer)، والتحقق من أن النظام يحمي الداتا ويرفض العملية.

---

## 51. Data Structure Pattern (Wholesale Order Details)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Structure |
| **Domain** | Information Domain |
| **Group** | Information Structure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** استنساخ لتحديد التركيبة البيانية المعقدة الخاصة بتفاصيل طلبات الجملة B2B.

### [3] Discussion
طلبات الجملة (Wholesale Orders) تختلف كلياً عن طلبات الأفراد، حيث تتطلب مصفوفات للمقاسات، وتوزيع الكميات على كل مقاس بصيغة Object (مثل S: 10, M: 20).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **structureName** | إلزامي (Mandatory) | Wholesale Order Breakdown. |
| **itemsOfInformation**| إلزامي (Mandatory) | اسم الموديل، الألوان، خريطة توزيع المقاسات (S, M, L, XL, XXL)، إجمالي الكمية، صور التصميم، وملاحظات التصنيع. |

### [5] Templates
"يجب أن تتكون التركيبة البيانية [structureName] من المعلومات التالية: [itemsOfInformation]."

### [6] Examples
* **مثال 1:** يجب أن تتكون التركيبة البيانية `Wholesale Order Breakdown` من المعلومات التالية: `اسم الموديل، الألوان المطلوبة كمصفوفة نصوص، خريطة توزيع الكميات على المقاسات، صور التصميم، وملاحظات التصنيع`. يتم حفظ هذا الهيكل داخل جدول `WholesaleOrder`.

### [7] Development & Testing Considerations
* **للمطور:** تم بناء חقل المقاسات كـ `type: Object` في Mongoose Schema ليسمح بمرونة التخزين `quantityPerSize: { S: 10, M: 20 }`.
* **المختبر:** التأكد من أن جمع قيم المقاسات يطابق دائماً حقل `totalQuantity` قبل الحفظ.

---

## 52. Data Structure Pattern (Worker Payroll Record)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Structure |
| **Domain** | Information Domain |
| **Group** | Information Structure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** لتحديد الهيكل المالي الخاص بعمال خط الإنتاج لضمان عدم ضياع المستحقات.

### [3] Discussion
حسابات العمال تتغير شهرياً بناءً على الخصومات والحضور.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **structureName** | إلزامي (Mandatory) | Worker Payroll Record. |
| **itemsOfInformation**| إلزامي (Mandatory) | الراتب الأساسي، الخصومات الشهرية، أيام الحضور، أيام الغياب. |

### [5] Templates
"يجب أن تتكون التركيبة البيانية [structureName] من المعلومات التالية: [itemsOfInformation]."

### [6] Examples
* **مثال 1:** يجب أن تتكون التركيبة البيانية `Worker Payroll Record` من المعلومات التالية: `الراتب الأساسي (salary)، الخصومات (deductions)، أيام الحضور، وأيام الغياب`.

---

## 53. Inquiry Pattern (Workers Attendance & Payroll List)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inquiry |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لعرض قائمة الموظفين وحالتهم المالية بشكل لحظي للإدارة.

### [3] Discussion
في شاشة الإدارة `OwnerDashboard`، هناك قسم خاص بالـ (Team & Payroll) يعرض كل عامل وحالته دون الحاجة لتنزيل تقرير مطبوع.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **inquiryName** | إلزامي (Mandatory) | Workers List Inquiry. |
| **businessIntent** | إلزامي (Mandatory) | مراجعة المستحقات وأيام الغياب للعمال لاتخاذ قرارات إدارية. |
| **informationToShow** | إلزامي (Mandatory) | اسم العامل، دوره، تاريخ البدء، الراتب، الخصومات، الصافي، الحضور والغياب والملاحظات. |
| **selectionCriteria** | اختياري (Optional) | الترتيب بالأحدث `sort({ createdAt: -1 })`. |

### [5] Templates
"يجب أن يوفر النظام استعلاماً باسم [inquiryName] لعرض [informationToShow]. الغرض منه هو [businessIntent]. يمكن تصفية البيانات باستخدام المعايير التالية: [selectionCriteria]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام استعلاماً باسم `Workers List Inquiry` لعرض `تفاصيل الرواتب والحضور والغياب`. الغرض منه هو `مراجعة المستحقات`. يتم عرض البيانات مرتبة حسب `الأحدث في التسجيل`.

---

## 54. Calculation Formula Pattern (Wholesale Order Total Price)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** لتحديد كيفية احتساب إجمالي تكلفة أمر التشغيل الخاص بالمصانع (الجملة).

### [3] Discussion
في طلب الجملة، لا يوجد سعر مسبق. الإدارة تحدد `pricePerPiece` ثم يتم حساب المجموع بضربه في الكمية الكلية.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | Wholesale Total Price. |
| **formula** | إلزامي (Mandatory) | Total Price = pricePerPiece * totalQuantity. |
| **variables** | إلزامي (Mandatory) | سعر القطعة، إجمالي عدد القطع المطلوبة. |

### [5] Templates
"يجب حساب [valueName] باستخدام المعادلة التالية: [formula]، حيث تمثل المتغيرات القيم الآتية: [variables]."

### [6] Examples
* **مثال 1:** يجب حساب `إجمالي فاتورة المصنع (Wholesale Total Price)` باستخدام المعادلة التالية: `Total Price = pricePerPiece * totalQuantity`، حيث تمثل المتغيرات `سعر القطعة المعتمد من الإدارة` و `إجمالي الكمية المطلوبة للموديل بجميع مقاساته`.

### [7] Development & Testing Considerations
* **للمطور:** تم تطبيق هذه المعادلة في مسار `PUT /wholesale-orders/:id`، بحيث إذا تم تحديث السعر، يتغير الـ `totalPrice` تلقائياً قبل الحفظ في الداتا بيس.

---

## 55. User Interface Pattern (Login Interface)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** تعريف واجهة الدخول الأساسية التي تفتح الباب للمستخدمين للدخول للسيستم.

### [3] Discussion
هذه الواجهة مخصصة للعملاء والإدارة للتحقق من الهوية واستخراج الـ Token.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | User Login Interface. |
| **interfacePurpose**| إلزامي (Mandatory) | مصادقة المستخدمين وتأمين الدخول للمنصة. |
| **users** | إلزامي (Mandatory) | المستخدمون المسجلون (Customers, Admins, Owners). |
| **informationInput**| إلزامي (Mandatory) | البريد الإلكتروني، كلمة المرور. |
| **informationOutput**| إلزامي (Mandatory) | رسائل الخطأ، أو تحويل مباشر للوحة التحكم. |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة مستخدم باسم `Login Interface` بغرض `تأمين الدخول`. تقبل المدخلات: `البريد الإلكتروني وكلمة المرور`. وتعرض المخرجات: `رسائل خطأ في حال البيانات المغلوطة (Invalid Credentials)`.

---

## 56. User Interface Pattern (Signup Interface)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** تعريف الواجهة التي تقوم بجمع بيانات العميل الجديد لفتح حساب.

### [3] Discussion
في كود `SignUp.tsx`، نقوم بجمع معلومات شخصية لإكمال نمط הـ User Registration.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | User Registration Interface. |
| **interfacePurpose**| إلزامي (Mandatory) | تسجيل العملاء الجدد وإنشاء هويات رقمية لهم. |
| **users** | إلزامي (Mandatory) | زوار الموقع. |
| **informationInput**| إلزامي (Mandatory) | الاسم الكامل، البريد، رقم الهاتف، كلمة المرور. |
| **informationOutput**| إلزامي (Mandatory) | إشعار بالنجاح والتحويل لصفحة الدخول. |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة مستخدم باسم `Signup Interface`. تقبل المدخلات: `الاسم، الإيميل، التليفون، وكلمة المرور`. يجب أن تقوم الواجهة بإظهار `عين (Eye icon)` لتمكين المستخدم من رؤية كلمة المرور أثناء كتابتها، وتعرض إشعاراً بنجاح العملية.

---

## 57. Data Maintenance Pattern (Edit Wholesale Order)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** لتحديد الوظائف المتاحة لتعديل الطلبات الضخمة من قبل الإدارة والمصنع.

### [3] Discussion
أحياناً يتم الاتفاق على تغيير الكميات أو الألوان بعد إنشاء الطلب بين المصنع والأتيليه. يوفر السيستم إمكانية تعديل الـ Wholesale Order.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Wholesale Order. |
| **functions** | إلزامي (Mandatory) | تعديل السعر، تعديل الألوان، تغيير الكميات بالمقاس، وتحديث الحالة. |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة لـ `تعديل السعر، الألوان، تغيير كميات المقاسات، وتحديث حالة الإنتاج` لـ `Wholesale Order`، وذلك عبر واجهة Modal منبثقة في الداشبورد.

---

## 58. Data Longevity Pattern (Worker Records)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Longevity |
| **Domain** | Information Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** لتحديد متطلبات الاحتفاظ بملفات العاملين السابقين للأغراض القانونية.

### [3] Discussion
حتى إذا تم حذف العامل (Soft Delete أو إبعاده من الواجهة)، يجب الاحتفاظ بتاريخ تعيينه ورواتبه في السجلات لمدة قانونية محددة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **dataDescription**| إلزامي (Mandatory) | Worker Profile & Payroll History. |
| **mannerOfStorage**| إلزامي (Mandatory) | Online / Archived Database. |
| **retentionDuration**| إلزامي (Mandatory) | 5 سنوات (كحد أدنى). |
| **durationStartTrigger**| إلزامي (Mandatory) | من تاريخ مغادرة العامل للورشة. |

### [5] Templates
"يجب تخزين بيانات [dataDescription] بصورة [mannerOfStorage] لمدة [retentionDuration] محسوبة من [durationStartTrigger]."

### [6] Examples
* **مثال 1:** يجب تخزين بيانات `Worker Profile` بصورة `مؤرشفة` لمدة `5 سنوات` محسوبة من `تاريخ حذف أو إنهاء تعاقد العامل` للرجوع إليها في المنازعات المالية.

---

## 59. Specific Authorization Pattern (Wholesale Order Pricing)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Specific Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** حظر الوصول للتسعير التجاري عن المصانع وتخصيصه لمالك المشروع فقط.

### [3] Discussion
في شاشة `FactoryDashboard.tsx` الخاصة بالمصنع الشريك، يظهر الطلب بدون تسعير. لا يمكن للمصنع أن يضع سعر القطعة `pricePerPiece` من حسابه بأي طريقة برمجية، وهذه عملية محمية (Authorized) لحساب הـ Owner.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **userRole** | إلزامي (Mandatory) | حساب FactoryClient. |
| **requiredRole** | إلزامي (Mandatory) | Owner/Admin. |
| **failureAction** | إلزامي (Mandatory) | تعطيل الحقل `disabled` في الـ UI، ورفض التحديث في הـ API. |

### [5] Templates
"يجب على النظام منع الوصول إلى [المسار / لوحة التحكم] وحجب البيانات تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق [الدور المصرح له]."

### [6] Examples
* **مثال 1:** يجب على النظام منع الوصول وتعديل حقل `(pricePerPiece) لطلبيات الجملة` وحجب هذه الصلاحية تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق `الإدارة (Owner)`، ولا يُسمح للمصنع بتسعير طلباته بنفسه.

---

## 60. ID Pattern (Factory Client Username)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | ID |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** تحديد كيفية إنشاء المعرف الفريد الخاص بدخول المصانع (Username).

### [3] Discussion
المصانع في Fluffy لا تدخل بالـ Email كالأفراد، بل يتم تخصيص `Username` فريد لهم من قبل الإدارة لتسجيل الدخول به في بوابتهم.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **ownerEntityName**| إلزامي (Mandatory) | Factory Client. |
| **idForm** | إلزامي (Mandatory) | نص (String) مكون من حروف وأرقام، بدون مسافات. |
| **scopeOfUniqueness**| إلزامي (Mandatory)| فريد على مستوى جدول (FactoryClient). |
| **howAllocated** | إلزامي (Mandatory) | يتم تعيينه يدوياً من الإدارة عبر لوحة التحكم. |

### [5] Templates
"يجب أن يمتلك كل [ownerEntityName] معرّفاً فريداً بهيئة [idForm] يتم توليده بواسطة [howAllocated]. يجب أن يكون المعرّف فريداً على مستوى [scopeOfUniqueness]."

### [6] Examples
* **مثال 1:** يجب أن يمتلك كل `عميل مصنع (Factory Client)` معرّفاً فريداً بهيئة `اسم مستخدم نصي (Username)` يتم توليده وتعيينه بواسطة `الإدارة (Owner)`. يجب أن يكون المعرّف فريداً على مستوى `جميع الكيانات الشريكة B2B في النظام`.

### [7] Development & Testing Considerations
* **المختبر:** التحقق من أن محاولة تسجيل مصنعين بنفس الـ Username تسبب خطأ بـ الكود `11000 Duplicate Key Error` المبرمج في `server.js`.

---

## 61. Inquiry Pattern (Product Catalog Inquiry)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inquiry |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لعرض قائمة المنتجات المتاحة للزوار والعملاء مع إمكانية الفرز والتصفية للاختيار والشراء.

### [3] Discussion
في الواجهة الأمامية للمتجر، يقوم العميل بتصفح المنتجات عبر استعلام ديناميكي. مسار `/products` في الـ Backend يقبل متغيرات تصفية (Query Parameters) لإرجاع المنتجات المطلوبة فقط بناءً على القسم.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **inquiryName** | إلزامي (Mandatory) | Product Catalog Inquiry. |
| **businessIntent** | إلزامي (Mandatory) | استعراض المنتجات المتاحة بغرض التسوق والشراء. |
| **informationToShow** | إلزامي (Mandatory) | صورة المنتج، الاسم، السعر، حالة التوفر (inStock)، والتقييم. |
| **selectionCriteria** | اختياري (Optional) | التصفية بواسطة قسم المنتج `category` (مثال: All, Tops, Bottoms). |

### [5] Templates
"يجب أن يوفر النظام استعلاماً باسم [inquiryName] لعرض [informationToShow]. الغرض منه هو [businessIntent]. يمكن تصفية البيانات باستخدام المعايير التالية: [selectionCriteria]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام استعلاماً باسم `Product Catalog Inquiry` لعرض `الصور والأسماء والأسعار للزوار`. الغرض منه هو `التسوق`. يمكن تصفية البيانات باستخدام `القسم (Category)` بحيث يتم جلب بيانات قسم محدد فقط من قاعدة البيانات لتقليل الحمل.

### [7] Development & Testing Considerations
* **للمطور:** في مسار الـ `GET /products`، تم استخدام `req.query.category` لعمل فلتر ديناميكي `if (category !== 'All') filter.category = category`.
* **المختبر:** التحقق من أن اختيار قسم غير موجود يعرض قائمة فارغة بشكل سليم ولا يتسبب في تحطم الصفحة.

---

## 62. Calculation Formula Pattern (Wholesale Total Quantity)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** لتحديد كيفية تجميع العدد الكلي لقطع الموديل الواحد في طلبات المصانع (الجملة) بناءً على تفصيل المقاسات.

### [3] Discussion
لا يقوم المصنع بإدخال حقل "إجمالي الكمية" يدوياً في الداشبورد الخاص به، بل يقوم بإدخال الكمية المطلوبة من كل مقاس (S, M, L, XL, XXL)، ويقوم النظام بحساب الإجمالي آلياً لتجنب الأخطاء البشرية.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | Wholesale Total Quantity. |
| **formula** | إلزامي (Mandatory) | Total Quantity = Sum(S, M, L, XL, XXL). |
| **variables** | إلزامي (Mandatory) | الكمية المدخلة لكل مقاس على حدة. |

### [5] Templates
"يجب حساب [valueName] باستخدام المعادلة التالية: [formula]، حيث تمثل المتغيرات القيم الآتية: [variables]."

### [6] Examples
* **مثال 1:** يجب حساب `إجمالي عدد القطع لطلب الجملة` باستخدام المعادلة التالية: `Total Quantity = S + M + L + XL + XXL`، حيث تمثل المتغيرات `الكميات التي تم إدخالها لكل مقاس على حدة في كائن quantityPerSize`.

### [7] Development & Testing Considerations
* **للمطور:** تم تنفيذ ذلك في الـ Frontend بملف `FactoryDashboard.tsx` عبر الدالة `Object.values(sizes).reduce((a,b) => a+b, 0)` قبل الإرسال للباك إند.

---

## 63. Data Maintenance Pattern (Record Factory Payment)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** وظيفة صيانة لسجل المصنع تسمح للإدارة بتوثيق المبالغ المالية المسددة نقدياً أو بنكياً.

### [3] Discussion
عندما يقوم المصنع بسداد جزء من مديونيته للمتجر، يقوم الأونر بتسجيل هذه الدفعة لتقليل الدين عبر مسار `POST /factory-clients/:id/payment`.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Factory Client Profile. |
| **functions** | إلزامي (Mandatory) | إضافة قيمة مالية إلى حقل المدفوعات (paidAmount). |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة لـ `زيادة حقل (paidAmount) بقيمة المبلغ المُسدد` لـ `حساب المصنع الشريك (Factory Client)`، مما يساهم ديناميكياً في تقليل مديونيته.

### [7] Development & Testing Considerations
* **للمطور:** يجب استخدام عملية الزيادة `$inc: { paidAmount: Number(amount) }` في `mongoose` لضمان تحديث السجل بشكل ذري (Atomic) حتى لو كان هناك أكثر من أدمن يقومون بالتحديث في نفس الوقت.

---

## 64. User Interface Pattern (Owner Dashboard Main Interface)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** تعريف واجهة التحكم المركزية التي تتيح للإدارة الإشراف على النظام بالكامل.

### [3] Discussion
شاشة الـ `OwnerDashboard.tsx` هي لوحة القيادة (Control Panel). تحتوي على تبويبات (Tabs) وإحصائيات علوية سريعة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | Owner Main Dashboard Interface. |
| **interfacePurpose**| إلزامي (Mandatory) | الإدارة الشاملة للمنتجات، العمال، المصانع، والطلبات. |
| **users** | إلزامي (Mandatory) | المستخدمون بصلاحية Owner و Admin. |
| **informationInput**| إلزامي (Mandatory) | التنقل بين التبويبات (Tabs) والضغط على أزرار التعديل. |
| **informationOutput**| إلزامي (Mandatory) | الإحصائيات (Stat Cards) والجداول الخاصة بكل قسم. |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة تحكم مركزية باسم `Owner Dashboard`. متاحة فقط للإدارة. وتعرض المخرجات: `بطاقات إحصائية بالأعلى للمبيعات والديون، ونظام تبويبات (Tabs) لعرض جداول المنتجات والموظفين بشكل منفصل`.

---

## 65. Data Longevity Pattern (Password Reset Token)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Longevity |
| **Domain** | Information Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** تحديد العمر الزمني (Expiration) لرموز الأمان (Tokens) المؤقتة قبل أن يتم إبطالها للحماية من الاختراقات.

### [3] Discussion
عندما يطلب العميل إعادة تعيين كلمة المرور، النظام يقوم بتوليد Token وحفظه في جدول `User`. لأسباب أمنية، يجب ألا يظل هذا الرمز صالحاً للأبد.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **dataDescription**| إلزامي (Mandatory) | Password Reset Token & Expiry Date. |
| **mannerOfStorage**| إلزامي (Mandatory) | داخل سجل المستخدم في قاعدة البيانات. |
| **retentionDuration**| إلزامي (Mandatory) | 10 دقائق (10 * 60 * 1000 مللي ثانية). |
| **durationStartTrigger**| إلزامي (Mandatory) | من لحظة توليد الرمز وطلب الاسترجاع. |

### [5] Templates
"يجب تخزين بيانات [dataDescription] بصورة [mannerOfStorage] لمدة [retentionDuration] محسوبة من [durationStartTrigger]."

### [6] Examples
* **مثال 1:** يجب تخزين `رمز إعادة تعيين كلمة المرور` بصورة `مؤقتة داخل سجل العميل` لمدة `10 دقائق فقط` محسوبة من `لحظة طلب العميل لإعادة التعيين`. بعد هذه المدة يُعتبر الرمز ملغياً (Expired).

### [7] Development & Testing Considerations
* **للمطور:** في دالة `createPasswordResetToken` داخل `userSchema`، تم ضبط التوقيت بدقة كالتالي: `this.passwordResetExpires = Date.now() + 10 * 60 * 1000;`.

---

## 66. Specific Authorization Pattern (Token Validation)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Specific Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ لحماية مسارات تحديث كلمات المرور بحيث لا تعبر إلا برمز سري صالح (Token Authorization).

### [3] Discussion
المسار `PATCH /users/reset-password/:token` يتطلب توثيقاً خاصاً، وهو أن يكون الـ Token الممرر في الرابط مطابقاً للـ Hash الموجود في قاعدة البيانات، وأن يكون تاريخ انتهاء الصلاحية أطول من الوقت الحالي.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **userRole** | إلزامي (Mandatory) | حائز الـ Token. |
| **requiredRole** | إلزامي (Mandatory) | يجب أن يمتلك Token صالحاً وغير منتهي الصلاحية. |
| **failureAction** | إلزامي (Mandatory) | إرجاع خطأ "الرابط غير صالح أو انتهت صلاحيته". |

### [5] Templates
"يجب على النظام منع الوصول إلى [المسار / لوحة التحكم] وحجب البيانات تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق [الدور المصرح له]."

### [6] Examples
* **مثال 1:** يجب على النظام منع الوصول وتحديث كلمة المرور في مسار `reset-password/:token` تماماً إلا إذا كان المستخدم يحمل `رمز أمان (Token) صالحاً رياضياً وغير منتهي الصلاحية ($gt: Date.now())`.

---

## 67. Data Structure Pattern (Customer Order Shipping Address)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Structure |
| **Domain** | Information Domain |
| **Group** | Information Structure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** استنساخ لتحديد التركيبة البيانية الخاصة ببيانات العميل اللوجستية التي تُمرر مع كل طلب.

### [3] Discussion
بيانات التوصيل ليست مجرد نص، بل هي هيكل (Structure) يُستخدم لحساب مصاريف الشحن ولتوجيه المندوب، ويتم التحقق من اكتمالها قبل الحفظ.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **structureName** | إلزامي (Mandatory) | Shipping Address Details. |
| **itemsOfInformation**| إلزامي (Mandatory) | اسم العميل (customerName)، الهاتف (phone)، العنوان التفصيلي (address)، والمحافظة (governorate). |

### [5] Templates
"يجب أن تتكون التركيبة البيانية [structureName] من المعلومات التالية: [itemsOfInformation]."

### [6] Examples
* **مثال 1:** يجب أن تتكون التركيبة البيانية `Shipping Address Details` من المعلومات التالية: `اسم العميل، رقم هاتف صحيح، عنوان تفصيلي، واسم المحافظة التي تُحدد تكلفة الشحن`.

### [7] Development & Testing Considerations
* **للمطور:** في مسار الـ `POST /orders`، تم وضع شرط صارم للتحقق من عدم فراغ هذه الحقول (باستخدام الدالة `.trim()`) لإرجاع خطأ `400 Bad Request` في حال غياب أحدهم.

---

## 68. Transaction Pattern (Client Payment Transaction)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Transaction |
| **Domain** | Data Entity Domain |
| **Group** | Business Entities Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** لتعريف المعاملات المالية الجزئية (الدفعات) التي تؤثر على الرصيد النهائي للكيانات.

### [3] Discussion
إضافة دفعة مالية لحساب مصنع شريك تعتبر "معاملة". ورغم أن المشروع الحالي يحفظها كزيادة مباشرة في حقل `paidAmount` بدلاً من جدول منفصل، إلا أنها من الناحية الهندسية والوظيفية تمثل حدثاً مالياً.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **transactionName** | إلزامي (Mandatory) | Factory Client Payment. |
| **informationToStore**| إلزامي (Mandatory) | المبلغ المسدد (Amount) ومعرف العميل. |

### [5] Templates
"يجب أن يقوم النظام بحفظ المعلومات التالية عن كل معاملة من نوع [transactionName]: [informationToStore]."

### [6] Examples
* **مثال 1:** يجب أن يقوم النظام بمعالجة وحفظ كل معاملة من نوع `Factory Client Payment` وتتضمن `المبلغ المالي المُدخل (Amount) ومعرّف الكيان (Client ID)` لتحديث السجلات.

---

## 69. Report Pattern (Customer Order Email Receipt)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Report |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ للنمط لتوصيف التقارير المجمعة التي يتم توليدها بصيغة ثابتة (HTML) وإرسالها للمستخدمين كمستند نهائي لا يقبل التعديل (إيصال الدفع).

### [3] Discussion
بعد إتمام الطلب، يقوم الـ Backend بتوليد كود HTML منسق يحتوي على الفاتورة وتفاصيل المنتجات، ويُرسله عبر خدمة البريد الإلكتروني. هذا الـ HTML هو في جوهره تقرير (Report) عن المعاملة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **reportName** | إلزامي (Mandatory) | Order Confirmation Receipt (Email). |
| **businessIntent** | إلزامي (Mandatory) | تزويد العميل بإثبات موثق للشراء يحتوي على المواصفات والأسعار. |
| **informationToShow** | إلزامي (Mandatory) | أسماء المنتجات، ألوانها، مقاساتها، الكمية، سعر القطعة. |
| **totalingLevels** | اختياري (Optional) | إجمالي ثمن البضاعة، مصاريف الشحن، والمجموع الكلي النهائي. |

### [5] Templates
"يجب أن يوفر النظام تقريراً باسم [reportName] لعرض [informationToShow]. الغرض من التقرير هو [businessIntent]. يجب أن يتضمن التقرير إجماليات لـ [totalingLevels]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام تقريراً يرسل كبريد إلكتروني باسم `إيصال تأكيد الطلب`. الغرض منه `تزويد العميل بإثبات شراء`. ويتضمن إجماليات لـ `(قيمة المنتجات، مصاريف الشحن، الإجمالي الكلي)`.

### [7] Development & Testing Considerations
* **للمطور:** تم بناء التقرير باستخدام `Template Literals` في `orderController.js`، مع استخدام دالة فرعية `hexToName` لترجمة أكواد الألوان إلى أسماء مقروءة للعميل في التقرير.

---

## 70. Data Maintenance Pattern (Automatic Stock Decrement)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** استنساخ للنمط لتوصيف الصيانة الآلية (Automatic Maintenance) للبيانات التي تحدث كرد فعل لمعاملة أخرى.

### [3] Discussion
عندما يشتري عميل 3 قطع من فستان، يجب أن ينقص المخزون الكلي للفستان بمقدار 3، ويزيد عداد المبيعات (soldCount) بمقدار 3 آلياً دون تدخل الإدارة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Product Stock. |
| **functions** | إلزامي (Mandatory) | إنقاص المخزون (stock) وزيادة عداد المبيعات (soldCount). |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة آلية لـ `إنقاص كمية المخزون وزيادة مرات البيع (soldCount)` لـ `المنتجات (Product Stock)`، ويتم تشغيل هذه الوظيفة فور نجاح إنشاء معاملة الطلب.

### [7] Development & Testing Considerations
* **للمطور:** في مسار الطلبات، يتم تنفيذ هذا عبر `await Product.findByIdAndUpdate(id, { $inc: { stock: -item.quantity, soldCount: item.quantity } })`. استخدام `$inc` يمنع تداخل الطلبات إذا تم طلب نفس المنتج من قبل مستخدمين مختلفين في نفس الثانية.

---

## 71. Data Structure Pattern (Product Review)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Structure |
| **Domain** | Information Domain |
| **Group** | Information Structure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** لتحديد التركيبة البيانية لتعليقات وتقييمات العملاء على المنتجات.

### [3] Discussion
التقييمات تساعد في اتخاذ قرار الشراء. تتكون المراجعة من نص وتقييم رقمي واسم العميل.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **structureName** | إلزامي (Mandatory) | Product Review Record. |
| **itemsOfInformation**| إلزامي (Mandatory) | اسم العميل (Name)، قيمة التقييم (Rating)، ونص التعليق (Text). |

### [5] Templates
"يجب أن تتكون التركيبة البيانية [structureName] من المعلومات التالية: [itemsOfInformation]."

### [6] Examples
* **مثال 1:** يجب أن تتكون التركيبة البيانية `Product Review Record` من المعلومات التالية: `اسم العميل، قيمة التقييم بالنجوم، ونص التعليق والمراجعة`.

### [7] Development & Testing Considerations
* **للمطور:** تم تطبيق هذه الهيكلة حالياً في `ProductDetail.tsx` داخل واجهة المستخدم (حفظ محلي). للتوسع مستقبلاً، يجب إنشاء `ReviewSchema` كـ Sub-document داخل الـ `Product` في MongoDB.

---

## 72. Data Type Pattern (Rating Score)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Type |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** استنساخ للنمط لفرض قيود صارمة على قيمة "التقييم" لمنع إدخال قيم غير منطقية.

### [3] Discussion
التقييم في النظام يجب أن يكون عدد صحيح من 1 إلى 5 نجوم ولا يقبل الكسور أو الأرقام السالبة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **dataTypeName** | إلزامي (Mandatory) | Rating Score. |
| **form** | إلزامي (Mandatory) | رقم صحيح (Integer). |
| **constraints** | اختياري (Optional) | أن يكون محصوراً بين 1 و 5 (inclusive). |

### [5] Templates
"يجب أن يكون نوع البيانات [dataTypeName] بهيئة [form]. يجب أن تُعرض البيانات للمستخدم بصيغة [displayFormat]، مع تطبيق القيود التالية: [constraints]."

### [6] Examples
* **مثال 1:** يجب أن يكون نوع البيانات `Rating Score` بهيئة `رقم صحيح`. ويُعرض للمستخدم بصيغة `أيقونات نجوم مضيئة (Stars)`، مع تطبيق القيود التالية: `يجب ألا يقل عن 1 ولا يزيد عن 5`.

---

## 73. Calculation Formula Pattern (Average Product Rating)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف كيفية حساب التقييم الإجمالي للمنتج بناءً على جميع المراجعات.

### [3] Discussion
المنتج قد يحصل على 10 تقييمات مختلفة، لذا يجب حساب المتوسط (Average) لعرضه للعملاء كتقييم عام للمنتج.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | Average Product Rating. |
| **formula** | إلزامي (Mandatory) | مجموع كل التقييمات / عددها الكلي. |
| **variables** | إلزامي (Mandatory) | قيم التقييمات الفردية، وعدد المراجعات. |

### [5] Templates
"يجب حساب [valueName] باستخدام المعادلة التالية: [formula]، حيث تمثل المتغيرات القيم الآتية: [variables]."

### [6] Examples
* **مثال 1:** يجب حساب `متوسط تقييم المنتج` باستخدام المعادلة التالية: `Math.round(Sum(ratings) / Total Reviews)`، حيث تمثل المتغيرات `مجموع قيم النجوم لجميع المراجعات` و `إجمالي عدد المراجعات المكتوبة للمنتج`.

### [7] Development & Testing Considerations
* **للمطور:** في الكود الحالي بـ `ProductDetail.tsx`، يتم حسابها ديناميكياً باستخدام `reviews.reduce((sum, r) => sum + r.rating, 0) / reviews.length`.

---

## 74. User Interface Pattern (Shopping Cart Interface)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف الواجهة التي تتيح للعميل استعراض المنتجات المحددة وتعديلها قبل إتمام الدفع.

### [3] Discussion
شاشة `Cart.tsx` هي من أهم الواجهات الحيوية. توفر للعميل نظرة شاملة على اختياراته (اللون والمقاس لكل قطعة) مع القدرة على حذفها أو زيادتها.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | Shopping Cart Interface. |
| **interfacePurpose**| إلزامي (Mandatory) | استعراض ومراجعة المنتجات المختارة وتعديل كمياتها. |
| **users** | إلزامي (Mandatory) | الزوار والعملاء. |
| **informationInput**| إلزامي (Mandatory) | أزرار الزيادة (+)، النقصان (-)، وأزرار الحذف. |
| **informationOutput**| إلزامي (Mandatory) | قائمة المنتجات، سعر القطعة، التكلفة الإجمالية الفرعية. |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة مستخدم باسم `Shopping Cart Interface` بغرض `مراجعة المشتريات`. تقبل المدخلات: `تعديل الكميات صعوداً وهبوطاً أو حذف المنتج`. وتعرض المخرجات: `صورة المنتج، تفاصيل اللون والمقاس، التكلفة الفرعية، وإجمالي السلة`.

---

## 75. Calculation Formula Pattern (Cart Subtotal Calculation)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ لتحديد الحسبة الخاصة بمجموع البضاعة في السلة (بدون ضرائب أو شحن).

### [3] Discussion
مجموع السلة الفرعي يتغير لحظياً عند أي ضغطة زر لتعديل الكمية في الـ Cart.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | Cart Subtotal. |
| **formula** | إلزامي (Mandatory) | Sum of (Price * Quantity) for all items. |
| **variables** | إلزامي (Mandatory) | سعر القطعة، والكمية المطلوبة منها. |

### [5] Templates
"يجب حساب [valueName] باستخدام المعادلة التالية: [formula]، حيث تمثل المتغيرات القيم الآتية: [variables]."

### [6] Examples
* **مثال 1:** يجب حساب `قيمة المنتجات الإجمالية في السلة (Subtotal)` باستخدام المعادلة التالية: `جمع حاصل ضرب (سعر كل قطعة × الكمية المطلوبة)` لجميع عناصر السلة الحالية.

---

## 76. Inter-System Interface Pattern (n8n Webhook - Return Order)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inter-System Interface |
| **Domain** | Fundamental Domain |
| **Group** | Inter-system Interfaces |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ لتعريف واجهة إرسال التحديثات العكسية (المرتجعات وإلغاء الطلبات) للأنظمة الخارجية.

### [3] Discussion
في كود مسار الـ `restore-stock`، عند إلغاء طلب وإرجاع المخزون، يقوم النظام بعمل PING جديد لنفس الـ Webhook الخاص بـ `n8n` لإخبار قسم اللوجستيات بأن هذا الطلب قد لُغي (أو تم تحديثه) عبر إرسال `action: 'update'`.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | n8n Webhook Interface (Return Updates). |
| **components** | إلزامي (Mandatory) | Fluffy Backend و n8n Platform. |
| **interfacePurpose**| إلزامي (Mandatory) | إشعار نظام الأتمتة بإلغاء الطلب (Cancelled). |
| **initiator** | إلزامي (Mandatory) | النظام الخاص بنا (Backend). |

### [5] Templates
"يجب أن تكون هناك واجهة اتصال محددة بوضوح تسمى [interfaceName] بين [component1] و [component2]. يتم تفعيلها بواسطة [initiator] لغرض [interfacePurpose]."

### [6] Examples
* **مثال 1:** يجب أن تكون هناك واجهة اتصال باسم `n8n Webhook (Return Updates)` بين `السيستم` و `n8n`. يتم تفعيلها بواسطة `السيستم` لغرض `إرسال تحديثات عن الطلبات المرتجعة والملغاة وتغيير حالتها في جدول اللوجستيات الخارجي`.

---

## 77. Chronicle Pattern (Order Cancellation Event)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Chronicle |
| **Domain** | Data Entity Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** استنساخ لتسجيل وقوع حدث حساس كإلغاء الطلب المكتمل.

### [3] Discussion
إلغاء الطلب بعد نجاحه يؤثر على الأرباح وعلى المخزون، لذا فإن توثيق وتتبع هذا الحدث أمر بالغ الأهمية.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **occurrenceType** | إلزامي (Mandatory) | Order Cancellation & Stock Restoration. |
| **informationToRecord**| إلزامي (Mandatory) | Order ID، الحالة الجديدة، والكميات المرجعة. |
| **severity** | اختياري (Optional) | Normal. |

### [5] Templates
"يجب على النظام تسجيل حدث [occurrenceType] تلقائياً عند وقوعه. يجب أن تتضمن البيانات المسجلة: [informationToRecord]، ويُصنف الحدث بمستوى خطورة [severity]."

### [6] Examples
* **مثال 1:** يجب على النظام تسجيل حدث `إلغاء الطلب واسترجاع المخزون` تلقائياً عند وقوعه. البيانات المسجلة يجب أن تشمل: `معرف الطلب، الحالة الجديدة (ملغي Cancelled)، والكميات المسترجعة لكل منتج`، ويتم ذلك على مستوى سجلات السيرفر وقاعدة البيانات.

---

## 78. Data Maintenance Pattern (Adjust Cart Item Quantity)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف القيود والشروط الخاصة بتحديث كمية عنصر داخل سلة المشتريات.

### [3] Discussion
العميل لا يمكنه اختيار كمية تتجاوز الـ `stock` المتاح في الداتا بيس، ولا كمية أقل من 1.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Cart Item. |
| **functions** | إلزامي (Mandatory) | تحديث الكمية بالزيادة أو النقصان بحد أقصى (Available Stock). |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة لـ `تحديث الكمية المطلوبة بالزيادة أو النقصان بحيث لا تتجاوز أبداً الحد الأقصى للمخزون (Stock)` لـ `عنصر السلة (Cart Item)`. وإذا حاول العميل تجاوز الحد تظهر رسالة خطأ (Toast).

### [7] Development & Testing Considerations
* **للمطور:** في `Cart.tsx` تم وضع شرط الإيقاف: `disabled={!item.stock || item.quantity >= item.stock}`.

---

## 79. Technology Pattern (Frontend Framework)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Technology |
| **Domain** | Fundamental Domain |
| **Group** | Technical Constraints Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ لتحديد التقنيات المعتمدة لبناء واجهات المستخدم الأمامية للمشروع (UI Stack).

### [3] Discussion
لتحقيق أداء عالي وسلاسة في الانتقالات بين الشاشات وبناء مكونات قابلة لإعادة الاستخدام، اختار المشروع حزمة React الحديثة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **technologyType** | إلزامي (Mandatory) | Frontend User Interface. |
| **technologyName** | إلزامي (Mandatory) | React.js, Tailwind CSS, Framer Motion. |

### [5] Templates
"يجب أن يستخدم النظام تقنية [technologyName] لغرض بناء وتشغيل [technologyType]."

### [6] Examples
* **مثال 1:** يجب أن يستخدم النظام تقنية `React.js` و إطار تصميم `Tailwind CSS` ومكتبة الحركات `Framer Motion` لغرض بناء وتشغيل `واجهات المستخدم الأمامية (Frontend)` لضمان الأداء السلس (SPA).

---

## 80. Specific Authorization Pattern (Admin Dashboard Access)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Specific Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ لحماية صفحات الواجهة الأمامية الإدارية من المتطفلين عبر الـ Routes.

### [3] Discussion
لا يكفي حماية الـ Backend فقط، بل يجب إخفاء شاشات مثل `OwnerDashboard.tsx` عن المستخدمين العاديين، وطردهم للرئيسية إذا حاولوا الدخول للرابط.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **userRole** | إلزامي (Mandatory) | دور المستخدم المخزن في التخزين المحلي (localStorage). |
| **requiredRole** | إلزامي (Mandatory) | Owner أو Admin. |
| **failureAction** | إلزامي (Mandatory) | الطرد (Redirect) للصفحة الرئيسية وإظهار رسالة رفض. |

### [5] Templates
"يجب على النظام منع الوصول إلى [المسار / لوحة التحكم] وحجب البيانات تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق [الدور المصرح له]."

### [6] Examples
* **مثال 1:** يجب على النظام منع الوصول إلى `شاشات لوحة تحكم الإدارة بالواجهة الأمامية (OwnerDashboard)` وحجب محتواها تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق `الإدارة (owner) أو (admin)`. وإذا لم يتحقق الشرط يتم إرجاعه للرئيسية `(Navigate to "/")`.

### [7] Development & Testing Considerations
* **للمطور:** تم تطبيق ذلك بدقة عبر مكون `AdminRoute.tsx` الذي يعمل كـ Wrapper يحيط بمسارات الداشبورد ويقوم بفحص دور المستخدم قبل تمرير הـ `<Outlet />`.

---

## 81. Calculation Formula Pattern (Shipping Fee Evaluation)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ لتحديد كيفية استخراج وحساب قيمة الشحن ديناميكياً لكل طلب جديد بناءً على المحافظة.

### [3] Discussion
مصاريف الشحن ليست ثابتة لجميع العملاء. النظام يعتمد على خريطة (Map) مُعرفة مسبقاً في الإعدادات لاستخراج التكلفة بناءً على المحافظة المختارة في سلة المشتريات.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | Order Shipping Fee. |
| **formula** | إلزامي (Mandatory) | ShippingFee = ShippingRates[Governorate] || 0. |
| **variables** | إلزامي (Mandatory) | اسم المحافظة المختارة، وقاموس أسعار الشحن. |

### [5] Templates
"يجب حساب [valueName] باستخدام المعادلة التالية: [formula]، حيث تمثل المتغيرات القيم الآتية: [variables]."

### [6] Examples
* **مثال 1:** يجب حساب `تكلفة شحن الطلب` باستخدام المعادلة التالية: `البحث في قاموس الأسعار (ShippingRates) باستخدام مفتاح (Governorate)، وإذا لم يتم العثور عليها تُحسب كـ 0`، حيث تمثل المتغيرات `المحافظة التي اختارها العميل من القائمة المنسدلة`.

---

## 82. Data Maintenance Pattern (Reset Worker Month)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** لتعريف وظيفة إدارية تقوم بتنظيف وتصفير عدادات مؤقتة للكيانات لتبدأ دورة زمنية جديدة.

### [3] Discussion
بعد نهاية كل شهر وصرف رواتب العمال، تحتاج الإدارة إلى تصفير أيام الحضور والغياب والخصومات لتبدأ دورة شهرية جديدة للعامل دون حذفه من النظام.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Worker Profile. |
| **functions** | إلزامي (Mandatory) | تصفير (Reset) الخصومات، أيام الحضور، وأيام الغياب. |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة لـ `تصفير أيام الحضور (presentDays)، وأيام الغياب (absentDays)، والخصومات المالية (deductions) إلى الرقم صفر` لـ `ملف العامل (Worker Profile)`.

### [7] Development & Testing Considerations
* **للمطور:** في `OwnerDashboard.tsx` يتم استدعاء دالة `resetWorkerMonth` والتي تمرر الكائن `{ presentDays: 0, absentDays: 0, deductions: 0 }` لمسار التحديث `PUT /workers/:id`.

---

## 83. User Authentication Pattern (Factory Client Login)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Authentication |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ للنمط لتعريف آلية المصادقة الخاصة بالكيانات التجارية (B2B) والتي تختلف جذرياً عن العملاء الأفراد.

### [3] Discussion
المصانع لا تسجل حساباً بنفسها. يتم إنشاء الحساب من قبل `Owner`. ولذلك، تسجيل الدخول يعتمد على `username` مخصص بدلاً من البريد الإلكتروني.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **email** | إلزامي (Mandatory) | اسم المستخدم (Username) كبديل للمعرف. |
| **password** | إلزامي (Mandatory) | كلمة المرور المصرحة من الإدارة. |
| **token** | إلزامي (Mandatory) | حفظ كائن العميل في التخزين المحلي (Local Storage). |

### [5] Templates
"يجب على النظام عند استقبال طلب تسجيل الدخول، التحقق من صحة [البريد الإلكتروني] ومطابقة [كلمة المرور]، وتوليد [JWT Token] صالح وصادر من السيرفر."

### [6] Examples
* **مثال 1:** يجب على النظام عند استقبال طلب تسجيل دخول للكيانات التجارية، التحقق من صحة `اسم المستخدم (Username)` ومطابقة `كلمة المرور` في جدول `FactoryClient` وإرجاع بيانات الجلسة للمتصفح للسماح بدخول بوابة `FactoryDashboard`.

---

## 84. Data Structure Pattern (AI VTO Request Payload)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Structure |
| **Domain** | Information Domain |
| **Group** | Information Structure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف الهيكل البياني المطلوب إرساله إلى واجهات الذكاء الاصطناعي الخارجية لضمان عدم رفض الطلب (Bad Request).

### [3] Discussion
نموذج `fashn-vton-1.5` يتطلب هيكلة بيانات صارمة (صور كـ Blob/Base64 وتصنيف مُعالج) لكي يعمل بشكل صحيح.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **structureName** | إلزامي (Mandatory) | VTO Prediction Request Payload. |
| **itemsOfInformation**| إلزامي (Mandatory) | صورة الشخص (person_image)، صورة الملابس (garment_image)، التصنيف (category)، وإعدادات النموذج (guidance_scale, steps). |

### [5] Templates
"يجب أن تتكون التركيبة البيانية [structureName] من المعلومات التالية: [itemsOfInformation]."

### [6] Examples
* **مثال 1:** يجب أن تتكون التركيبة البيانية `VTO Prediction Request Payload` المُرسلة لسيرفر الذكاء الاصطناعي من: `صورة شخصية (Blob)، صورة القطعة، والتصنيف بعد معالجته نصياً إلى (tops, bottoms, one-pieces)`.

---

## 85. Inquiry Pattern (Factory Clients Roster)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inquiry |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ للنمط لتعريف واجهة استعلام الإدارة لعرض الكيانات التجارية وملخص ديونهم.

### [3] Discussion
الـ Owner يحتاج لمراقبة من هي المصانع التي سددت ومن عليها مديونية، مع إمكانية البحث السريع باسم الشركة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **inquiryName** | إلزامي (Mandatory) | Factory Clients Roster Inquiry. |
| **businessIntent** | إلزامي (Mandatory) | متابعة الشركاء الماليين وإجمالي المديونيات. |
| **informationToShow** | إلزامي (Mandatory) | اسم الشركة، المالك، إجمالي الديون، المدفوع، والمتبقي. |
| **selectionCriteria** | اختياري (Optional) | التصفية الحية (Live Search) باسم الشركة أو المالك. |

### [5] Templates
"يجب أن يوفر النظام استعلاماً باسم [inquiryName] لعرض [informationToShow]. الغرض منه هو [businessIntent]. يمكن تصفية البيانات باستخدام المعايير التالية: [selectionCriteria]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام استعلاماً باسم `Factory Clients Roster` لعرض `اسم الكيان التجاري، المالك، والمديونية المتبقية ديناميكياً`. الغرض منه هو `متابعة الحسابات المالية للمصانع`. يمكن تصفية البيانات باستخدام `حقل البحث الحر بالاسم (Search Bar)`.

---

## 86. Data Maintenance Pattern (Global Shipping Configuration)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** لتعريف وظيفة التحديث الخاصة بإعدادات النظام المركزية.

### [3] Discussion
إعدادات أسعار التوصيل `Shipping Rates` يتم تحديثها من الإدارة عبر مسار `PUT /shipping-rates`، وهي بيانات حيوية تؤثر على جميع الطلبات اللاحقة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Global Shipping Configuration. |
| **functions** | إلزامي (Mandatory) | تحديث (Update) واستبدال قاموس الأسعار (Rates Object). |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة لـ `تحديث وحفظ خريطة أسعار الشحن للمحافظات` لـ `إعدادات النظام العامة (Global Shipping Configuration)`.

---

## 87. Technology Pattern (Database Management System)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Technology |
| **Domain** | Fundamental Domain |
| **Group** | Technical Constraints Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: Yes |

### [2] Applicability
* **When to use:** استنساخ لتحديد التقنية المحددة للتعامل مع قاعدة البيانات والمكتبات المساعدة لفرض الهياكل.

### [3] Discussion
التعامل المباشر مع MongoDB قد يسبب عشوائية في البيانات. المشروع يستخدم مكتبة `Mongoose` كـ ODM (Object Data Modeling) لفرض `Schemas` صارمة قبل التخزين.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **technologyType** | إلزامي (Mandatory) | Database & Object Data Modeling. |
| **technologyName** | إلزامي (Mandatory) | MongoDB (Database) & Mongoose (ODM). |

### [5] Templates
"يجب أن يستخدم النظام تقنية [technologyName] لغرض بناء وتشغيل [technologyType]."

### [6] Examples
* **مثال 1:** يجب أن يستخدم النظام تقنية `قاعدة بيانات MongoDB ومكتبة Mongoose` لغرض بناء وتشغيل `هياكل البيانات والتحقق من صحتها (Validation) قبل تخزينها`.

---

## 88. Dynamic Capacity Pattern (Payload Size Limit)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Dynamic Capacity |
| **Domain** | Performance Domain |
| **Group** | Performance Constraints Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ لتحديد القيود على حجم حزم البيانات (Payload) المُستقبلة في طلب واحد لحماية السيرفر من الانفجار (Buffer Overflow).

### [3] Discussion
المستخدم يقوم برفع صور للمنتجات وصور لخدمة الـ VTO بصيغة `Base64` مما يعني نصوصاً عملاقة. يجب وضع سقف محدد للـ Payload.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **subject** | إلزامي (Mandatory) | حجم الطلب الوارد (API Request Body / Payload). |
| **capacity** | إلزامي (Mandatory) | بحد أقصى 50 ميجابايت (50mb). |

### [5] Templates
"يجب أن يكون النظام قادراً على دعم وتحمل [capacity] من [subject]."

### [6] Examples
* **مثال 1:** يجب أن يكون النظام قادراً على دعم وتحمل ما يصل إلى `50 ميجابايت` من `حجم الطلب الوارد (JSON & URLEncoded Body Payload)` لتسهيل رفع الصور بدقة عالية للذكاء الاصطناعي، ويتم رفض أي طلب يتجاوز هذا الحجم.

### [7] Development & Testing Considerations
* **للمطور:** في `server.js` تم ضبط القيد بنجاح عبر `app.use(express.json({ limit: '50mb' }));`.

---

## 89. Data Longevity Pattern (VTO Ephemeral Results)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Longevity |
| **Domain** | Information Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ للنمط لتحديد البيانات "سريعة الزوال" (Transient Data) التي يُمنع تخزينها لأسباب تتعلق بالخصوصية أو السعة.

### [3] Discussion
صور العملاء التي يتم رفعها لتجربة الملابس بالذكاء الاصطناعي (VTO) هي بيانات شخصية حساسة. النظام صُمم لكي لا يحفظها في قاعدة البيانات إطلاقاً، بل يمررها كـ Blob للـ AI ويعرض النتيجة للعميل فقط.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **dataDescription**| إلزامي (Mandatory) | الصور الشخصية للعميل في ميزة الـ VTO وصور النتيجة. |
| **mannerOfStorage**| إلزامي (Mandatory) | معالجة حية (In-memory/Client-side) دون تخزين في قاعدة البيانات. |
| **retentionDuration**| إلزامي (Mandatory) | 0 ثانية (لا يتم حفظها إطلاقاً). |
| **durationStartTrigger**| إلزامي (Mandatory) | فور انتهاء المعالجة وإرجاع الرد للعميل. |

### [5] Templates
"يجب تخزين بيانات [dataDescription] بصورة [mannerOfStorage] لمدة [retentionDuration] محسوبة من [durationStartTrigger]."

### [6] Examples
* **مثال 1:** يجب تخزين `الصور الشخصية للعميل وصور القياس الافتراضي الناتجة` بصورة `مؤقتة في ذاكرة الوصول العشوائي للعميل (Client-side)` لمدة `0 ثانية (تُمسح فوراً)` محسوبة من `لحظة إنهاء الطلب`؛ حيث يُحظر قطعياً تخزينها في الـ Database لحماية الخصوصية.

---

## 90. Specific Authorization Pattern (Record Client Payment)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Specific Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** استنساخ للنمط لحماية المعاملات المالية البينية (B2B Payments) من أن يتم توثيقها بواسطة أشخاص غير مخولين.

### [3] Discussion
مسار `POST /factory-clients/:id/payment` يقوم بزيادة رصيد المدفوعات للمصنع. لا يُسمح للمصنع أن يسجل دفعة لنفسه ليقلل ديونه، هذه الصلاحية تنحصر للإدارة فقط (Owner/Admin) بعد استلام النقدية فعلياً.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **userRole** | إلزامي (Mandatory) | دور المستخدم المُنفذ للطلب. |
| **requiredRole** | إلزامي (Mandatory) | حساب إدارة (Owner/Admin). |
| **failureAction** | إلزامي (Mandatory) | رفض الطلب وحماية جدول المدفوعات. |

### [5] Templates
"يجب على النظام منع الوصول إلى [المسار / لوحة التحكم] وحجب البيانات تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق [الدور المصرح له]."

### [6] Examples
* **مثال 1:** يجب على النظام منع الوصول وتعديل الداتا عبر `مسار تسجيل دفعات المصانع المالية` وحجب العملية تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق `الإدارة العليا (Owner أو Admin)`، ويُمنع أي مصنع من توثيق دفعة لنفسه.

---

## 91. Chronicle Pattern (Unhandled Server Errors)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Chronicle |
| **Domain** | Data Entity Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Yes; Affects database: Maybe |

### [2] Applicability
* **When to use:** لتحديد آلية النظام المركزية لالتقاط وتسجيل الأخطاء البرمجية الطارئة لضمان عدم توقف الخدمة بدون أثر.

### [3] Discussion
في نهاية ملف `server.js`، يوجد `Middleware` مركزي يلتقط أي خطأ غير معالج (Unhandled Error) ويقوم بطباعته أو تسجيله لكي يقوم المطور بمراجعته لاحقاً، مع إرجاع استجابة آمنة للمستخدم `500 Server Error`.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **occurrenceType** | إلزامي (Mandatory) | خطأ برمجي غير معالج (Unhandled Exception). |
| **informationToRecord**| إلزامي (Mandatory) | تفاصيل الخطأ (Stack Trace, Error Message). |
| **severity** | اختياري (Optional) | Severe (حرج). |

### [5] Templates
"يجب على النظام تسجيل حدث [occurrenceType] تلقائياً عند وقوعه. يجب أن تتضمن البيانات المسجلة: [informationToRecord]، ويُصنف الحدث بمستوى خطورة [severity]."

### [6] Examples
* **مثال 1:** يجب على النظام تسجيل حدث `خطأ برمجي غير معالج في الخادم` تلقائياً عند وقوعه. يجب أن تتضمن البيانات المسجلة: `رسالة الخطأ وتفاصيل الـ Stack Trace`، ويُصنف الحدث بمستوى خطورة `حرج (Severe)`، مع إبلاغ المستخدم بحدوث خطأ عام دون كشف تفاصيل السيرفر.

---

## 92. Calculation Formula Pattern (Total Revenue Calculation)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف كيفية تجميع القيم المالية للطلبات لاستخراج إجمالي إيرادات المشروع.

### [3] Discussion
شريط الإحصائيات في `OwnerDashboard.tsx` يعرض `Total Revenue`. هذه القيمة تُحسب ديناميكياً بجمع إجماليات جميع الطلبات الناجحة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | Total Revenue. |
| **formula** | إلزامي (Mandatory) | Sum of (totalAmount) for all orders. |
| **variables** | إلزامي (Mandatory) | إجمالي مبلغ كل طلب. |

### [5] Templates
"يجب حساب [valueName] باستخدام المعادلة التالية: [formula]، حيث تمثل المتغيرات القيم الآتية: [variables]."

### [6] Examples
* **مثال 1:** يجب حساب `إجمالي الإيرادات (Total Revenue)` باستخدام المعادلة التالية: `جمع (Sum) لكل قيم (totalAmount) من جميع الطلبات المسجلة`، حيث تمثل المتغيرات `الإجمالي النهائي الموثق في كل فاتورة`.

---

## 93. Data Maintenance Pattern (Update Wholesale Order Status)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Maintenance |
| **Domain** | User Function Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: Yes |

### [2] Applicability
* **When to use:** لتعريف الوظيفة التي تتيح للمصانع والإدارة تغيير حالة خط الإنتاج لطلب الجملة.

### [3] Discussion
الطلب يمر بمراحل: في انتظار التسعير، قيد الانتظار، جاري القص، جاري الخياطة، تم التسليم. واجهة `FactoryDashboard` توفر قائمة منسدلة (Dropdown) لتحديث هذه الحالة مباشرة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **entityName** | إلزامي (Mandatory) | Wholesale Order. |
| **functions** | إلزامي (Mandatory) | تحديث (Update) حالة الطلب (Status). |

### [5] Templates
"يجب أن يوفر النظام وظيفة لـ [functions] لـ [entityName]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام وظيفة لـ `تحديث حالة الإنتاج اللوجستية للطلب (مثل: جاري الخياطة أو تم التسليم)` لـ `طلب الجملة (Wholesale Order)`.

---

## 94. Specific Authorization Pattern (Protect Delete APIs)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Specific Authorization |
| **Domain** | Access Control Domain |
| **Group** | Access Control Infrastructure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Yes; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف القيود الأمنية على مسارات حذف الكيانات الأساسية من قاعدة البيانات.

### [3] Discussion
في الـ Backend، مسار `DELETE /workers/:id` هو مسار خطير. يجب ألا يتمكن أي شخص إلا الإدارة من إرسال طلب الحذف.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **userRole** | إلزامي (Mandatory) | دور المستخدم المُرسل للطلب. |
| **requiredRole** | إلزامي (Mandatory) | Owner / Admin. |
| **failureAction** | إلزامي (Mandatory) | الرفض التام (403 Forbidden). |

### [5] Templates
"يجب على النظام منع الوصول إلى [المسار / لوحة التحكم] وحجب البيانات تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق [الدور المصرح له]."

### [6] Examples
* **مثال 1:** يجب على النظام منع الوصول إلى `مسار حذف العمال من الخادم (DELETE /workers/:id)` وحجب تنفيذ العملية تماماً إلا إذا كان حساب المستخدم يحمل دوراً يطابق `الإدارة العليا (Owner/Admin)`.

---

## 95. Data Structure Pattern (System Settings Configuration)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Structure |
| **Domain** | Information Domain |
| **Group** | Information Structure Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: Maybe; Affects database: Yes |

### [2] Applicability
* **When to use:** لتعريف التركيبة البيانية لجدول الإعدادات العامة الذي يحتوي على بيانات متغيرة (Dynamic Keys).

### [3] Discussion
للسماح بحفظ أسعار الشحن المتغيرة، تم تصميم موديل `Settings.js` بحيث لا يحتوي على حقول ثابتة للمحافظات، بل يحتوي على حقل مرن `rates: Object`.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **structureName** | إلزامي (Mandatory) | System Settings Record. |
| **itemsOfInformation**| إلزامي (Mandatory) | نوع الإعداد (type)، وقاموس البيانات المرن (rates). |

### [5] Templates
"يجب أن تتكون التركيبة البيانية [structureName] من المعلومات التالية: [itemsOfInformation]."

### [6] Examples
* **مثال 1:** يجب أن تتكون التركيبة البيانية `System Settings Record` من المعلومات التالية: `نوع الإعداد (type: 'shipping' مثلاً)، وكائن مرن (rates) يحتوي على مفاتيح ديناميكية وقيم رقمية`.

---

## 96. Inquiry Pattern (Single Product Fetch)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Inquiry |
| **Domain** | User Function Domain |
| **Group** | Inquiry Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف وظيفة استعلام دقيقة تجلب تفاصيل كيان واحد بعينه لعرضه للعميل في صفحة مخصصة.

### [3] Discussion
عندما يضغط العميل على منتج، يفتح مسار `ProductDetail.tsx` الذي يستدعي الـ API المخصص لجلب تفاصيل هذا المنتج باستخدام معرّفه (ID).

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **inquiryName** | إلزامي (Mandatory) | Single Product Details Inquiry. |
| **businessIntent** | إلزامي (Mandatory) | عرض كافة مواصفات القطعة لتمكين العميل من اختيار المقاس واللون. |
| **informationToShow** | إلزامي (Mandatory) | كل بيانات المنتج، المخزون، الصور، والمواصفات. |
| **selectionCriteria** | اختياري (Optional) | التصفية بواسطة معرّف المنتج (Product ID). |

### [5] Templates
"يجب أن يوفر النظام استعلاماً باسم [inquiryName] لعرض [informationToShow]. الغرض منه هو [businessIntent]. يمكن تصفية البيانات باستخدام المعايير التالية: [selectionCriteria]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام استعلاماً باسم `Single Product Details Inquiry` لعرض `الوصف الشامل، مجموعة الصور، المقاسات والألوان المتاحة، والكمية الفعلية في المخزن`. الغرض منه هو `دعم قرار العميل الشرائي`. ويتم التصفية باستخدام `معرّف المنتج (ID) الممرر في الرابط`.

---

## 97. Calculation Formula Pattern (Total Workers Payroll)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Calculation Formula |
| **Domain** | Information Domain |
| **Group** | Information Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: Maybe; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف المعادلة المستخدمة لحساب الإجمالي المالي الذي يجب على الورشة تجهيزه لدفع رواتب الموظفين.

### [3] Discussion
في قسم `Team & Payroll` في الداشبورد، يظهر `Total Payroll` الذي يجمع الرواتب الأساسية ليكون المدير على علم بالسيولة المطلوبة.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **valueName** | إلزامي (Mandatory) | Total Payroll Liability. |
| **formula** | إلزامي (Mandatory) | Sum of (salary) for all registered workers. |
| **variables** | إلزامي (Mandatory) | قيمة الراتب لكل عامل. |

### [5] Templates
"يجب حساب [valueName] باستخدام المعادلة التالية: [formula]، حيث تمثل المتغيرات القيم الآتية: [variables]."

### [6] Examples
* **مثال 1:** يجب حساب `التزامات الرواتب الإجمالية للورشة (Total Payroll)` باستخدام المعادلة التالية: `مجموع (Sum) رواتب العمال الأساسية`، حيث تمثل المتغيرات `حقل الراتب (salary) الخاص بكل عامل مسجل في النظام`.

---

## 98. Data Longevity Pattern (Client Browser Session)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | Data Longevity |
| **Domain** | Information Domain |
| **Group** | Data Maintenance Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: No; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لتحديد المدة التي يتم فيها الاحتفاظ ببيانات الجلسة (Session) على جهاز المستخدم لتجنب تسجيل الدخول المتكرر.

### [3] Discussion
عندما يسجل المصنع أو العميل دخوله، يتم حفظ بياناته في `localStorage`. هذا يضمن بقاء جلسته نشطة حتى لو قام بإغلاق المتصفح وعاد لاحقاً، ما لم يسجل الخروج بنفسه.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **dataDescription**| إلزامي (Mandatory) | User / Factory Client Session Data. |
| **mannerOfStorage**| إلزامي (Mandatory) | تخزين محلي بالمتصفح (Local Storage). |
| **retentionDuration**| إلزامي (Mandatory) | حتى انتهاء صلاحية الـ JWT (90 يوماً). |
| **durationStartTrigger**| إلزامي (Mandatory) | من لحظة تسجيل الدخول الناجح. |

### [5] Templates
"يجب تخزين بيانات [dataDescription] بصورة [mannerOfStorage] لمدة [retentionDuration] محسوبة من [durationStartTrigger]."

### [6] Examples
* **مثال 1:** يجب تخزين بيانات `جلسة المستخدم والتوكن (Session)` بصورة `محلية في المتصفح (Local Storage)` لمدة `90 يوماً` محسوبة من `لحظة تسجيل الدخول` أو حتى يقوم المستخدم بتسجيل الخروج يدوياً.

---

## 99. User Interface Pattern (Form Validation Error Display)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف متطلب الواجهة في كيفية عرض الأخطاء للمستخدمين عند إدخال بيانات خاطئة أو عند رفض السيرفر لطلبهم.

### [3] Discussion
واجهة المستخدم لا يجب أن تتعطل إذا أدخل المستخدم بيانات مكررة (مثل إيميل موجود بالفعل). بل يجب أن تظهر رسالة واضحة، وهو ما يتم عبر المكونات مثل `Toast` أو `alert()`.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | Form Validation & Error Feedback. |
| **interfacePurpose**| إلزامي (Mandatory) | تنبيه المستخدم برفض البيانات مع توضيح السبب. |
| **users** | إلزامي (Mandatory) | جميع مستخدمي الواجهة. |
| **informationInput**| إلزامي (Mandatory) | بيانات خاطئة أو متعارضة. |
| **informationOutput**| إلزامي (Mandatory) | رسالة خطأ صريحة مستمدة من استجابة السيرفر. |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة باسم `تنبيهات الأخطاء (Error Feedback)` بغرض `توضيح سبب رفض البيانات المدخلة`. تقبل الواجهة المدخلات مثل `(محاولة تسجيل حساب بإيميل مكرر)` وتعرض المخرجات: `إشعار مرئي واضح يحمل رسالة السيرفر (البريد الإلكتروني مستخدم بالفعل)`.

---

## 100. User Interface Pattern (Wholesale Factory Portal)

### [1] Basic Details
| الحقل (Field) | القيمة المطابقة للمشروع (Value) |
| :--- | :--- |
| **Pattern Name** | User Interface |
| **Domain** | User Function Domain |
| **Group** | User Interface Group |
| **Anticipated Frequency** | - |
| **Classifications** | Functional: Yes; Pervasive: No; Affects database: No |

### [2] Applicability
* **When to use:** لتعريف الواجهة المتكاملة التي تخدم شركاء النجاح في الـ B2B وتوفر لهم أدوات إدخال وعرض خاصة.

### [3] Discussion
وهو مسك الختام! شاشة `FactoryDashboard.tsx` هي بمثابة تطبيق مصغر داخل التطبيق. تتيح للمصنع متابعة البضاعة المسلمة، الديون، ورفع طلبات تصنيع جديدة بالصور والمقاسات.

### [4] Content
| بند المعلومات (Item) | الحالة (Status) | الوصف والمعلومات المطلوبة في الكود (Description) |
| :--- | :--- | :--- |
| **interfaceName** | إلزامي (Mandatory) | Factory Wholesale Portal. |
| **interfacePurpose**| إلزامي (Mandatory) | إدارة دورة العمل بين المصنع والأتيليه (B2B). |
| **users** | إلزامي (Mandatory) | مدراء المصانع الشريكة (Factory Clients). |
| **informationInput**| إلزامي (Mandatory) | رفع صور الموديلات، تحديد الألوان، وتوزيع الكميات على المقاسات. |
| **informationOutput**| إلزامي (Mandatory) | إجمالي المديونية، سجل الطلبات، والتسعير المعتمد من الإدارة. |

### [5] Templates
"يجب أن يوفر النظام واجهة مستخدم باسم [interfaceName] بغرض [interfacePurpose]. يجب أن تكون الواجهة متاحة لـ [users]. يجب أن تقبل المدخلات التالية: [informationInput] وتعرض المخرجات التالية: [informationOutput]."

### [6] Examples
* **مثال 1:** يجب أن يوفر النظام واجهة مستخدم باسم `Factory Wholesale Portal` بغرض `تسهيل طلبات الجملة وتتبع المديونيات`. الواجهة متاحة لـ `المصانع الشريكة`. تقبل المدخلات: `توزيع مقاسات الموديل ورفع صوره`. وتعرض المخرجات: `كشف حساب مبسط للديون وسجل مفصل لطلبات التشغيل وحالتها`.
