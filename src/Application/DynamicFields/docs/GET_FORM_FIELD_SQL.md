```sql

SELECT Fields.JsonConfig,
       Fields.IsRequired,
       FT.Code,
       LL.Value as Name
FROM Fields
         INNER JOIN Categories ON Fields.CategoryId = Categories.Id
         INNER JOIN iranagah_UserAdmin.FieldTypes FT on Fields.FieldTypeId = FT.Id
         LEFT JOIN LanguageLocalizers LL ON LL.RelationId = Fields.Id AND LL.PropertyName = 'Name'
         INNER JOIN iranagah_UserAdmin.LanguageCodes LC on LL.LanguageCodeId = LC.Id AND LC.Code = 'ar-OM'
WHERE Fields.CategoryId IN
      (SELECT Id FROM Categories WHERE CategoryTypeId IN
                                       (SELECT Id FROM CategoryTypes WHERE Code = 'PLATE'))

```