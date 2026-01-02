# Software Test App

## Proje Hakkında

Yazılım Kalite Güvence Testi dersi için hazırlanmış bir .NET Core Web API projesi. Temel CRUD işlemleri ve test yapıları içeriyor.

### Teknolojiler

- C# / ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger
- xUnit

## Kurulum

1.  Repoyu klonlayın:
    ```bash
    git clone <repo-url>
    cd Software_Test_App
    ```

2.  Bağımlılıkları yükleyin:
    ```bash
    dotnet restore
    ```

3.  Veritabanını güncelleyin:
    ```bash
    cd Software_Test_App
    dotnet ef database update
    ```

4.  Uygulamayı çalıştırın:
    ```bash
    dotnet run
    ```

## API Endpoints

### Users
- `GET /api/Users` - Tüm kullanıcıları listeler
- `GET /api/Users/{id}` - Belirli bir kullanıcıyı getirir
- `POST /api/Users` - Yeni kullanıcı oluşturur
- `PUT /api/Users/{id}` - Kullanıcıyı günceller
- `DELETE /api/Users/{id}` - Kullanıcıyı siler

### Entries
- `GET /api/Entries` - Tüm girdileri listeler
- `GET /api/Entries/{id}` - Belirli bir girdiyi getirir
- `POST /api/Entries` - Yeni girdi oluşturur
- `PUT /api/Entries/{id}` - Girdiyi günceller
- `DELETE /api/Entries/{id}` - Girdiyi siler

### Reviews
- `GET /api/Reviews` - Tüm değerlendirmeleri listeler
- `GET /api/Reviews/{id}` - Belirli bir değerlendirmeyi getirir
- `POST /api/Reviews` - Yeni değerlendirme oluşturur
- `PUT /api/Reviews/{id}` - Değerlendirmeyi günceller
- `DELETE /api/Reviews/{id}` - Değerlendirmeyi siler

### Tags
- `GET /api/Tags` - Tüm etiketleri listeler
- `GET /api/Tags/{id}` - Belirli bir etiketi getirir
- `POST /api/Tags` - Yeni etiket oluşturur
- `PUT /api/Tags/{id}` - Etiketi günceller
- `DELETE /api/Tags/{id}` - Etiketi siler

### Search
- `GET /api/Search` - Arama geçmişini listeler
- `GET /api/Search/query?q={term}` - İçeriklerde arama yapar
- `POST /api/Search` - Arama kaydı oluşturur
- `PUT /api/Search/{id}` - Arama kaydını günceller
- `DELETE /api/Search/{id}` - Arama kaydını siler

## Swagger

https://localhost:7105/swagger
http://localhost:5137/swagger

## Testler

```bash
dotnet test
```

## Birim Testler

BirimTest.md dosyasındaki kurallara göre 21 birim test oluşturuldu.

### Testler

#### Model Testleri (ModelTests.cs)
1. **User_SetProperties_ReturnsCorrectValues** - User model property testi
2. **Review_SetProperties_ReturnsCorrectValues** - Review model property testi
3. **Tag_SetProperties_ReturnsCorrectValues** - Tag model property testi

#### Entry Testleri (EntryTests.cs)
4. **Entry_SetProperties_ReturnsCorrectValues** - Entry model property testi

#### EntriesController Testleri (EntriesControllerTests.cs)
5. **GetEntries_ReturnsAllEntries** - Tüm Entry'leri getirme testi (pozitif)
6. **GetEntry_ReturnsEntry_WhenExists** - ID ile Entry getirme testi (pozitif)
7. **GetEntry_ReturnsNotFound_WhenDoesNotExist** - Olmayan Entry testi (negatif)
8. **PostEntry_AddsEntry_WhenValid** - Geçerli Entry ekleme testi (pozitif)
9. **PostEntry_ReturnsBadRequest_WhenUserDoesNotExist** - Geçersiz UserId ile Entry ekleme (negatif)
10. **DeleteEntry_RemovesEntry_WhenExists** - Entry silme testi (pozitif)
11. **DeleteEntry_ReturnsNotFound_WhenDoesNotExist** - Olmayan Entry silme testi (negatif)

#### UsersController Testleri (UsersControllerTests.cs)
12. **GetUsers_ReturnsAllUsers** - Tüm User'ları getirme testi (pozitif)
13. **GetUser_ReturnsUser_WhenExists** - ID ile User getirme testi (pozitif)
14. **GetUser_ReturnsNotFound_WhenDoesNotExist** - Olmayan User testi (negatif)
15. **PostUser_AddsUser** - User ekleme testi (pozitif)
16. **DeleteUser_RemovesUser_WhenExists** - User silme testi (pozitif)
17. **DeleteUser_ReturnsNotFound_WhenDoesNotExist** - Olmayan User silme testi (negatif)

#### ReviewsController Testleri (ReviewsControllerTests.cs)
18. **GetReviews_ReturnsAllReviews** - Tüm Review'ları getirme testi (pozitif)
19. **PostReview_AddsReview_WhenValid** - Geçerli Review ekleme testi (pozitif)
20. **PostReview_ReturnsBadRequest_WhenEntryDoesNotExist** - Geçersiz EntryId ile Review ekleme (negatif)
21. **DeleteReview_RemovesReview** - Review silme testi (pozitif)

### Test Kapsami

- Model Testleri: 4 test
- Controller Testleri: 17 test
  - GET, POST, DELETE islemleri
  - Foreign Key validation

### Kullanilan Kutuphaneler
- xUnit (2.9.3)
- Microsoft.EntityFrameworkCore.InMemory (10.0.1)
- Microsoft.NET.Test.Sdk (17.14.1)

Test Dosyalari:
- ModelTests.cs
- EntryTests.cs
- EntriesControllerTests.cs
- UsersControllerTests.cs
- ReviewsControllerTests.cs

## Entegrasyon Testleri

EntegrasyonTest.md kurallarina göre 15 entegrasyon testi var.

### Test Listesi

1. **GetUsers_ReturnsAllUsers_IntegrationTest** - GET endpoint testi
2. **PostUser_CreatesNewUser_IntegrationTest** - POST endpoint testi  
3. **PutUser_UpdatesExistingUser_IntegrationTest** - PUT endpoint testi
4. **DeleteUser_RemovesUser_IntegrationTest** - DELETE endpoint testi
5. **GetEntry_ReturnsNotFound_WhenDoesNotExist_IntegrationTest** - 404 hata testi
6. **PostEntry_ReturnsBadRequest_WhenInvalidUserId_IntegrationTest** - 400 hata testi
7. **User_Entry_Relationship_IntegrationTest** - User-Entry ilişki testi
8. **Entry_Review_Relationship_IntegrationTest** - Entry-Review ilişki testi
9. **Entry_Tag_Relationship_IntegrationTest** - Entry-Tag ilişki testi
10. **ComplexCRUD_UserWithEntriesAndReviews_IntegrationTest** - Kompleks CRUD işlemleri
11. **DatabaseCRUD_MultipleEntries_IntegrationTest** - Çoklu Entry CRUD testi
12. **Tag_CRUD_Operations_IntegrationTest** - Tag CRUD işlemleri
13. **PostReview_ReturnsBadRequest_WhenInvalidEntryId_IntegrationTest** - Review 400 hata testi
14. **PostTag_ReturnsBadRequest_WhenInvalidEntryId_IntegrationTest** - Tag 400 hata testi
15. **GetUser_ReturnsNotFound_WhenDoesNotExist_IntegrationTest** - User 404 hata testi

### Kapsam

- API endpoint testleri (GET, POST, PUT, DELETE)
- Veritabani CRUD islemleri
- Iliskili tablolar arasi islemler (User-Entry, Entry-Review, Entry-Tag)
- Hata durumlari (404, 400)

### Kullanilan
- Microsoft.AspNetCore.Mvc.Testing (10.0.1)
- Microsoft.EntityFrameworkCore.InMemory (10.0.1)
- xUnit (2.9.3)

Test Dosyasi: `IntegrationTests.cs`

```bash
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

## Sistem Testleri

sistemTestleri.md kurallarina göre 8 uctan uca sistem testi var.

### Test Senaryolari

1. CompleteUserJourney - Kullanıcı kaydı, Entry, Review, Tag ekleme
2. ContentManagement - Listeleme, görüntüleme, güncelleme, silme
3. SearchWorkflow - Entry oluşturma, arama yapma, sonuçları görüntüleme
4. MultiUserInteraction - Çoklu kullanıcı, çapraz review'lar
5. ComplexBusinessScenario - Kompleks iş akışları
6. ErrorHandling - Geçersiz verilerle hata yönetimi
7. BulkDataProcessing - Toplu veri işleme
8. DataIntegrity - İlişkili kaynaklar, veri tutarlılığı

### Kullanilan
- Microsoft.AspNetCore.Mvc.Testing
- Microsoft.EntityFrameworkCore.InMemory
- xUnit

Test Dosyasi: `SystemTests.cs`

```bash
dotnet test --filter "FullyQualifiedName~SystemTests"
```