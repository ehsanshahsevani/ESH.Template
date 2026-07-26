# Category Type Field Mapping System

این سیستم به شما امکان می‌دهد که فیلدهای دینامیک را بر اساس نوع دسته‌بندی (CategoryType) مدیریت کنید.

---

## ساختار

### 1. CategoryTypes
فایل: `Constants/CategoryTypes.cs`

انواع دسته‌بندی‌های موجود:
- `PLATE` - پلاک خودرو
- `PHONE` - شماره تلفن
- `PROPERTY` - املاک
- `OTHER` - سایر

### 2. CategoryTypeFieldMapping
فایل: `Seed/CategoryTypeFieldMapping.cs`

این کلاس نگاشت (mapping) بین نوع دسته‌بندی و فیلدهای مربوط به آن را تعریف می‌کند.

**فیلدهای پلاک (Plate):**
- PlateNumberPart - الجزء الرقمي للوحة (عدد 5 رقمی)
- PlateLetter - حرف اللوحة (کاراکترهای عمان و...)
- PlateStatus - حالة اللوحة (خصوصی/تجاری)
- PlatePrice - سعر اللوحة
- LocationField - الموقع/المنطقة
- Image - صورة اللوحة
- Description - الوصف

**فیلدهای شماره تلفن (Phone):**
- PhoneBody - رقم الهاتف
- PhoneOperator - شركة الاتصالات
- Price - السعر
- LocationField - الموقع
- Image - صورة
- Description - الوصف

**فیلدهای املاک (Property):**
- Title - العنوان
- Price - السعر
- Amount - المبلغ
- LocationField - الموقع
- Image - صور الملكية
- Description - الوصف

### 3. CategoryTypeFieldProvider
فایل: `Seed/CategoryTypeFieldProvider.cs`

این کلاس سرویس اصلی است که اطلاعات کامل فیلدها را برای هر نوع دسته‌بندی برمی‌گرداند.

---

## نحوه استفاده

### مثال 1: دریافت فیلدها برای نوع پلاک

```csharp
var provider = new CategoryTypeFieldProvider();
var plateFields = provider.GetFieldsForCategoryType(CategoryTypes.Plate);

foreach (var field in plateFields)
{
    Console.WriteLine($"{field.TitleEn}: {field.Code}");
    // خروجی:
    // Plate Number: PLATE_NUMBER_PART
    // Plate Letter: PLATE_LETTER
    // Plate Status: PLATE_STATUS
    // Plate Price: PLATE_PRICE
    // Location: LOCATION
    // Image: IMAGE
    // Description: DESCRIPTION
}
```

### مثال 2: بررسی اعتبار فیلد

```csharp
var provider = new CategoryTypeFieldProvider();

// آیا این فیلد برای پلاک معتبر است؟
bool isValid = provider.ValidateField(CategoryTypes.Plate, FieldTypes.PlateNumberPart);
// نتیجه: true

bool isInvalid = provider.ValidateField(CategoryTypes.Plate, FieldTypes.PhoneBody);
// نتیجه: false
```

### مثال 3: دریافت یک فیلد خاص

```csharp
var provider = new CategoryTypeFieldProvider();
var field = provider.GetFieldForCategoryType(CategoryTypes.Plate, FieldTypes.PlatePrice);

if (field != null)
{
    Console.WriteLine($"عنوان عربی: {field.TitleAr}");
    Console.WriteLine($"عنوان انگلیسی: {field.TitleEn}");
    Console.WriteLine($"نوع داده: {field.DataType}");
    Console.WriteLine($"تنظیمات: {field.JsonConfig}");
}
```

### مثال 4: استفاده در سرویس Seed

```csharp
public async Task CreateFieldsForCategory(string categoryCode, string categoryType)
{
    var provider = new CategoryTypeFieldProvider();
    var fields = provider.GetFieldsForCategoryType(categoryType);
    
    foreach (var fieldSeed in fields)
    {
        var field = new Field
        {
            CategoryId = categoryId,
            FieldTypeCode = fieldSeed.Code,
            IsRequired = true, // یا false بسته به نیاز
            DisplayOrder = fields.IndexOf(fieldSeed)
        };
        
        await _unitOfWork.FieldRepository.AddAsync(field);
    }
    
    await _unitOfWork.SaveAsync();
}
```

---

## یادداشت‌های مهم

1. **تنظیمات فیلدها:** هر فیلد دارای تنظیمات خاص خود است که در `FieldTypeSeedData` تعریف شده است.

2. **پلاک عمان:** 
   - شماره پلاک: عدد 5 رقمی (1 تا 99999)
   - کاراکترهای پلاک: باید در جدول MultiValue ذخیره شود
   - وضعیت: خصوصی یا تجاری

3. **شماره تلفن عمان:**
   - فرمت: 8 رقمی
   - پیش‌شماره‌های معتبر: 901, 902, 903, 904, 906
   - مثال: 90123456

4. **قیمت‌ها:**
   - واحد: ریال عمان (OMR)
   - دقت: 3 رقم اعشار

---

## توسعه آینده

اگر نیاز به اضافه کردن نوع دسته‌بندی جدید دارید:

1. ثابت جدید را در `CategoryTypes.cs` اضافه کنید
2. فیلدهای مربوطه را در `CategoryTypeFieldMapping.cs` تعریف کنید
3. در صورت نیاز، فیلدهای جدید را در `FieldTypeSeedData.cs` اضافه کنید

