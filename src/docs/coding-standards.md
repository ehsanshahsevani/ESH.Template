<div dir="rtl">


# مستند استانداردهای برنامه‌نویسی – شرکت  

** نسخه کامل و استاندارد – ویژه برنامه‌نویسان **

این سند شامل مجموعه قوانین، الگوها و استانداردهایی است که در تمام پروژه‌های شرکت باید *بدون استثنا* رعایت شوند.

---

# اصل طلایی

## «هر آنچه که ما نمی‌نویسیم و کامپایلر برداشت می‌کند را به صراحت می‌نویسیم»

ما اجازه نمی‌دهیم کامپایلر رفتار مخفی یا پیش‌فرض را اعمال کند.  
هر چیزی که ممکن است مبهم باشد باید صریح نوشته شود:

- مقداردهی اولیه
- استفاده از null  
- مقدار بازگشتی  
- بلاک‌ها  
- ساختارهای کنترلی  
- پراپرتی‌ها  

### مثال غلط:

```csharp
public string Name { get; set; }
```

### مثال صحیح:

```csharp
public string Name { get; set; } = string.Empty;
```

---

# قانون براکت‌ها – حذف ممنوع

ما **به هیچ عنوان** اجازه استفاده از شرط‌ها و حلقه‌های بدون بلاک `{ }` را نمی‌دهیم.

## مثال‌های غلط:

```csharp
if (user != null)
    DoSomething();

for (int i = 0; i < 10; i++)
    Console.WriteLine(i);

if (isActive) return true;
```

## مثال‌های صحیح:

```csharp
if (user != null)
{
    DoSomething();
}

for (int i = 0; i < 10; i++)
{
    Console.WriteLine(i);
}

if (isActive)
{
    return true;
}
```

---

# قانون عدم بازگشت مستقیم – **NO DIRECT RETURN**

در هیچ تابعی return مستقیم وجود ندارد.  
ابتدا در متغیر `result` قرار می‌دهیم و سپس return انجام می‌شود.

## مثال غلط:

```csharp
public int GetAge()
{
    return 25;
}
```

##### مثال زیر در سیستم به کل استفاده نمیشود و همین رفتار و همین ممنوعیت شامل حال پراپرتی های تک لاینی میشود
```csharp
public int GetAge() => 25;
```


## مثال صحیح:

```csharp
public int GetAge()
{
    var result = 25;
    return result;
}
```

---

# قوانین Linq – **بسیار مهم**

## اصول:

- هر شرط باید در یک `Where` جدا نوشته شود.  
- هیچ شرطی داخل `FirstOrDefault` یا `Any` و ... نباید نوشته شود.  
- از نوشتن چند شرط در یک Where پرهیز کنید.  
- شرط‌های چندگانه را در چند لاین جدا بنویسید.

---

## مثال غلط – استفاده از شرط داخل FirstOrDefault:

```csharp
var result = users
    .FirstOrDefault(x => x.Age > 18 && x.IsActive == true);
```

## مثال صحیح:

```csharp
var result = users
    .Where(x => x.Age > 18)
    .Where(x => x.IsActive == true)
    .FirstOrDefault();
```

---

## مثال غلط – چند شرط در یک Where:

```csharp
var items = products
    .Where(p => p.Price > 100 && p.IsAvailable == true && p.Stock > 0)
    .ToList();
```

## مثال صحیح:

```csharp
var items = products
    .Where(p => p.Price > 100)
    .Where(p => p.IsAvailable == true)
    .Where(p => p.Stock > 0)
    .ToList();
```

---

## مثال غلط – شرط داخل Any:

```csharp
var exists = users.Any(x => x.IsActive == true);
```

## مثال صحیح:

```csharp
var exists = users
    .Where(x => x.IsActive == true)
    .Any();
```

---

# پرهیز کامل از شرط‌های تک‌خطی (Ternary بدون بلاک)

## مثال غلط:

```csharp
var result = isExist == true ? "yes" : "no";
```

## مثال صحیح:

```csharp
string result;

if (isExist == true)
{
    result = "yes";
}
else
{
    result = "no";
}

return result;
```

---

# بررسی صریح true و false – **بدون علامت !**

استفاده از ! ممنوع است.

## مثال‌های غلط:

```csharp
if (!isActive)
{
}

if (!user.IsVerified)
{
}
```

## مثال‌های صحیح:

```csharp
if (isActive == false)
{
}

if (user.IsVerified == false)
{
}
```

---

# DI و ReadOnly Property – بدون استفاده از readonly

## مثال غلط:

```csharp
private readonly IProfileService _profileService;
```

## مثال صحیح:

```csharp
public IProfileService ProfileService { get; }
```

---

# استاندارد ریجن‌ها در کنترلر

## ریجن مخصوص هر اکشن  
فرمت **الزامی**:

```csharp
#region POST: /update-image-profile

/// <summary>
/// این اکشن تصویر پروفایل کاربر را بروزرسانی می‌کند.
/// </summary>
[HttpPost("update-image-profile")]
public async Task<IActionResult> UpdateProfileImage([FromForm] ProfileImageResponseViewModel model)
{
    var result =
        await ProfileService.UpdateImageAsync(model);
    
    return result;
}

#endregion /POST: /update-image-profile
```

---

## ریجن سازنده:

```csharp
#region DI Settings & Constructor

public ProfileController(IProfileService profileService)
{
    ProfileService = profileService;
}

#endregion
```

---

## ریجن توابع خصوصی:

```csharp
#region private function

private bool ValidateImage(IFormFile file)
{
    var result = file.Length < 2000000;
    return result;
}

#endregion
```

---

# Summary فارسی – اجباری

### مثال صحیح:

```csharp
/// <summary>
/// این اکشن اطلاعات یک کاربر را بر اساس شناسه وارد شده برمی‌گرداند.
/// </summary>
```

### مثال غلط:

```csharp
/// get user info
```

---

# استرینگ های با شخصیت

### مثال غلط:

```csharp
string profile =
    GetObjectWithThisName("profile");
```

### مثال صحیح:

```csharp
string profile =
    GetObjectWithThisName(nameof(Profile));
```

یا

```csharp
string profile =
    GetObjectWithThisName(
        nameof(ESH.Resources.DataDictionary.Profile));
```

در صورت نیاز به حروف کوچک:
```csharp
string profile = 
    GetObjectWithThisName(
        nameof(ESH.Resources.DataDictionary.Profile).ToLower());
```

# قوانین ارث‌بری کلاس‌ها و سازنده پیش‌فرض (Default Constructor)

## ارث‌بری کلاس‌ها – اجباری  
تمام کلاس‌ها باید **از یک کلاس دیگر ارث‌بری کنند**.  
در ساده‌ترین حالت، اگر کلاس پدر خاصی تعریف نشده باشد باید **به‌صورت صریح** از `object` ارث‌بری شود.

### مثال صحیح:

```csharp
public class User : object
{
}
```

### مثال غلط:

```csharp
public class User
{
}
```

---

## سازنده پیش‌فرض – الزامی  
تمام کلاس‌ها باید یک **سازنده پیش‌فرض (Default Constructor)** داشته باشند، حتی اگر خالی باشد.  
این کار باعث استانداردسازی ساختار کلاس و جلوگیری از ابهام می‌شود.

### مثال صحیح:

```csharp
public class User : object
{
    public User()
    {
    }
}
```

---

## مواردی که سازنده پیش‌فرض می‌تواند حذف یا private شود

### حالت 1: زمانی که سازنده پیش‌فرض مشکل‌ساز است  
اگر وجود سازنده خالی باعث ساخت شیء بدون مقداردهی لازم شود،  
می‌توان سازنده پیش‌فرض را حذف کرد.

### حالت 2: زمانی که سازنده کامل‌تری وجود دارد  
اگر کلاس یک سازنده دقیق و کامل دارد، می‌توان سازنده پیش‌فرض را **private** کرد  
یا به‌طور کامل حذف نمود.

### مثال – سازنده پیش‌فرض private:

```csharp
public class User : object
{
    private User()
    {
    }

    public User(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string Name { get; set; }
    public int Age { get; set; }
}
```

### مثال – سازنده پیش‌فرض حذف شده:

```csharp
public class User : object
{
    public User(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
```

---

# پایان سند  

این سند همواره آپدیت خواهد شد و رعایت آن برای همه اعضای تیم الزامی است.

لطفا در هر بخش از پروژه خلاف این موراد رویت شد وقت بزارید و اصلاح بفرمایید **بدون توجه به اینکه آن کد را شما نوشته اید یا خیر**

<div>