# سیستم مدیریت فیلدهای دینامیک بر اساس نوع دسته‌بندی
## Dynamic Fields Management System Based on Category Type

---

## ✅ خلاصه کار انجام شده

### 1. فایل‌های ایجاد شده:

#### الف) کلاس‌های اصلی:
1. **`CategoryTypes.cs`** - تعریف انواع دسته‌بندی‌ها
   - مسیر: `Announcement/Application/DynamicFields/Constants/CategoryTypes.cs`
   - نوع‌ها: PLATE، PHONE، PROPERTY، OTHER

2. **`CategoryTypeFieldMapping.cs`** - نگاشت فیلدها به نوع دسته‌بندی
   - مسیر: `Announcement/Application/DynamicFields/Seed/CategoryTypeFieldMapping.cs`
   - عملکرد: تعیین اینکه هر نوع دسته‌بندی چه فیلدهایی دارد

3. **`CategoryTypeFieldProvider.cs`** - سرویس اصلی دریافت فیلدها
   - مسیر: `Announcement/Application/DynamicFields/Seed/CategoryTypeFieldProvider.cs`
   - عملکرد: دریافت اطلاعات کامل فیلدها بر اساس نوع

#### ب) فایل‌های کمکی:
4. **`CategoryTypeFieldUsageExample.cs`** - مثال‌های استفاده
   - مسیر: `Announcement/Application/DynamicFields/Seed/CategoryTypeFieldUsageExample.cs`

5. **`README.md`** - مستندات کامل
   - مسیر: `Announcement/Application/DynamicFields/Seed/README.md`

#### ج) تست‌ها:
6. **`CategoryTypeFieldMappingTests.cs`** - تست‌های Mapping
7. **`CategoryTypeFieldProviderTests.cs`** - تست‌های Provider
   - مسیر: `Announcement/Shared/UnitTest/DynamicFields/`
   - وضعیت: ✅ **21 تست موفق**

---

## 📋 فیلدهای تعریف شده برای هر نوع

### 🚗 پلاک خودرو (PLATE)
```csharp
1. PlateNumberPart    // الجزء الرقمي للوحة (1-99999)
2. PlateLetter        // حرف اللوحة (عمان، ...)
3. PlateStatus        // حالة اللوحة (خصوصی/تجاری)
4. PlatePrice         // سعر اللوحة
5. LocationField      // الموقع
6. Image              // صورة اللوحة
7. Description        // الوصف
```

### 📱 شماره تلفن (PHONE)
```csharp
1. PhoneBody          // رقم الهاتف (901xxxxx، 902xxxxx)
2. PhoneOperator      // شركة الاتصالات
3. Price              // السعر
4. LocationField      // الموقع
5. Image              // صورة
6. Description        // الوصف
```

### 🏠 املاک (PROPERTY)
```csharp
1. Title              // العنوان
2. Price              // السعر
3. Amount             // المبلغ/المساحة
4. LocationField      // الموقع
5. Image              // صور الملكية
6. Description        // الوصف
```

### 📦 سایر (OTHER)
```csharp
1. Title              // العنوان
2. Price              // السعر
3. LocationField      // الموقع
4. Image              // صورة
5. Description        // الوصف
```

---

## 🚀 نحوه استفاده

### مثال 1: دریافت فیلدها برای پلاک
```csharp
using DynamicFields.Constants;
using DynamicFields.Seed;

var provider = new CategoryTypeFieldProvider();

// دریافت تمام فیلدها
var plateFields = provider.GetFieldsForCategoryType(CategoryTypes.Plate);

foreach (var field in plateFields)
{
    Console.WriteLine($"{field.TitleEn} ({field.Code})");
    Console.WriteLine($"Data Type: {field.DataType}");
    Console.WriteLine($"Config: {field.JsonConfig}");
    Console.WriteLine("---");
}
```

### مثال 2: بررسی اعتبار فیلد
```csharp
var provider = new CategoryTypeFieldProvider();

// آیا PlateNumberPart برای PLATE معتبر است؟
bool isValid = provider.ValidateField(
    CategoryTypes.Plate, 
    FieldTypes.PlateNumberPart
);
// نتیجه: true

// آیا PhoneBody برای PLATE معتبر است؟
bool isInvalid = provider.ValidateField(
    CategoryTypes.Plate, 
    FieldTypes.PhoneBody
);
// نتیجه: false
```

### مثال 3: دریافت یک فیلد خاص
```csharp
var provider = new CategoryTypeFieldProvider();

var field = provider.GetFieldForCategoryType(
    CategoryTypes.Plate, 
    FieldTypes.PlatePrice
);

if (field != null)
{
    Console.WriteLine($"Arabic: {field.TitleAr}");
    Console.WriteLine($"English: {field.TitleEn}");
}
```

### مثال 4: استفاده در InitialData/Seed
```csharp
public async Task CreateFieldsForCategory(
    Guid categoryId, 
    string categoryType)
{
    var provider = new CategoryTypeFieldProvider();
    var fields = provider.GetFieldsForCategoryType(categoryType);
    
    int displayOrder = 1;
    foreach (var fieldSeed in fields)
    {
        // جستجوی FieldType
        var fieldType = await _unitOfWork
            .FieldTypeRepository
            .FindByCodeAsync(fieldSeed.Code);
            
        if (fieldType == null) continue;
        
        // ایجاد Field
        var field = new Field
        {
            CategoryId = categoryId,
            FieldTypeId = fieldType.Id,
            IsRequired = ShouldBeRequired(fieldSeed.Code),
            DisplayOrder = displayOrder++
        };
        
        await _unitOfWork.FieldRepository.AddAsync(field);
    }
    
    await _unitOfWork.SaveAsync();
}

private bool ShouldBeRequired(string fieldCode)
{
    // تعیین اینکه کدام فیلدها اجباری هستند
    return fieldCode switch
    {
        FieldTypes.PlateNumberPart => true,
        FieldTypes.PhoneBody => true,
        FieldTypes.Price => true,
        _ => false
    };
}
```

---

## 🧪 اجرای تست‌ها

```powershell
# اجرای تمام تست‌های CategoryType
cd "C:\Users\Acer\Desktop\Projects\OmanMarket.2026\Announcement\Shared\UnitTest"
dotnet test --filter "FullyQualifiedName~CategoryType"

# نتیجه: ✅ 21 تست موفق
```

---

## 📊 نتایج تست

```
Test Summary:
- Total:     21
- Succeeded: 21 ✅
- Failed:    0
- Skipped:   0
- Duration:  3.4s
```

---

## 🔧 کامپایل پروژه

```powershell
cd "C:\Users\Acer\Desktop\Projects\OmanMarket.2026\Announcement\Application\DynamicFields"
dotnet build

# نتیجه: ✅ Build succeeded
```

---

## 📝 نکات مهم

### 1. پلاک عمان:
- شماره پلاک: 1 تا 99999 (5 رقمی)
- کاراکترها: "عُمان" و کاراکترهای خاص در جدول MultiValue
- وضعیت: خصوصی (Private) یا تجاری (Commercial)
- قیمت: با واحد ریال عمان (OMR) و 3 رقم اعشار

### 2. شماره تلفن عمان:
- فرمت: 8 رقمی
- پیش‌شماره‌های معتبر: 901, 902, 903, 904, 906
- مثال: 90123456

### 3. توسعه آینده:
برای اضافه کردن نوع جدید:
1. ثابت جدید در `CategoryTypes.cs`
2. نگاشت فیلدها در `CategoryTypeFieldMapping.cs`
3. فیلدهای جدید در `FieldTypeSeedData.cs` (در صورت نیاز)

---

## ✨ مزایای این رویکرد

1. **تمرکز:** همه فیلدها در یک جا مدیریت می‌شوند
2. **اعتبارسنجی:** بررسی خودکار معتبر بودن فیلد برای نوع
3. **قابلیت تست:** تست‌های جامع برای اطمینان از صحت
4. **توسعه‌پذیری:** اضافه کردن نوع جدید بسیار ساده است
5. **تایپ‌سیف:** استفاده از ثابت‌ها به جای رشته‌های مستقیم

---

## 📞 پشتیبانی

برای هرگونه سوال یا مشکل، به فایل‌های زیر مراجعه کنید:
- `README.md` - مستندات کامل
- `CategoryTypeFieldUsageExample.cs` - مثال‌های استفاده
- تست‌ها - برای درک بهتر عملکرد

---

**وضعیت پروژه:** ✅ آماده برای استفاده
**تاریخ:** 17 فوریه 2026

