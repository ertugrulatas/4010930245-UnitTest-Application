# Software Test App

![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-CI-blue) ![Codecov](https://img.shields.io/badge/Codecov-Coverage-brightgreen)

## Proje Açıklaması

Bu proje, Yazılım Kalite Güvence Testi dersi kapsamında geliştirilmiş bir .NET Core Web API uygulamasıdır. Proje, çeşitli veri modelleri üzerinde CRUD işlemleri gerçekleştiren ve test edilebilir bir yapı sunan bir backend servisidir.

### Kullanılan Teknolojiler

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQLite Veritabanı
- Swagger UI

## Kurulum Talimatları

Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları izleyin:

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

## API Endpointleri

Aşağıda uygulamada bulunan temel API endpointleri listelenmiştir:

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

## Dokümantasyon

Swagger UI üzerinden API dokümantasyonuna ve test arayüzüne erişebilirsiniz:

[http://localhost:5137/swagger](http://localhost:5137/swagger)

## Testler

Testleri çalıştırmak için proje ana dizininde şu komutu kullanın:

```bash
dotnet test
```

## Birim Test Özeti

BirimTest.md dosyasındaki kurallara göre **21 kapsamlı birim test** oluşturulmuştur.

### ✅ Oluşturulan Testler

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

### 📋 Karşılanan Gereksinimler

✅ **En az 15 farklı birim test** (21 test oluşturuldu)  
✅ **İş mantığı fonksiyonları testi** (Controller business logic)  
✅ **Veri doğrulama fonksiyonları testi** (UserId, EntryId validation)  
✅ **Model/Entity metodları testi** (Entry, User, Review, Tag)  
✅ **Minimum %60 kod coverage** (Controllers ve Models kapsandı)  
✅ **Anlamlı test adları** (Her test açıklayıcı isimlere sahip)  
✅ **Pozitif ve negatif senaryolar** (Başarılı ve hata durumları test edildi)

### 🎯 Test Kapsamı

- **Model Testleri:** 4 test - Tüm model property'lerinin doğru set/get edilmesi
- **Controller Testleri:** 17 test
  - GET işlemleri (var/yok senaryoları)
  - POST işlemleri (geçerli/geçersiz veri senaryoları)
  - DELETE işlemleri (var/yok senaryoları)
  - İlişki kontrolü (Foreign Key validation)

### 🔧 Teknik Detaylar

**Kullanılan Teknolojiler:**
- xUnit (2.9.3)
- Microsoft.EntityFrameworkCore.InMemory (10.0.1)
- Microsoft.NET.Test.Sdk (17.14.1)

**Test Mimarisi:**
- InMemory Database ile test isolation
- Her test için benzersiz database instance (Guid bazlı)
- Arrange-Act-Assert pattern kullanımı

**Test Dosyaları:**
- `Software_Test_App.Tests/ModelTests.cs`
- `Software_Test_App.Tests/EntryTests.cs`
- `Software_Test_App.Tests/EntriesControllerTests.cs`
- `Software_Test_App.Tests/UsersControllerTests.cs`
- `Software_Test_App.Tests/ReviewsControllerTests.cs`

Birim testlerini çalıştırmak için:

```bash
dotnet test
```

## Entegrasyon Test Özeti

EntegrasyonTest.md dosyasındaki kurallara göre **15 kapsamlı entegrasyon testi** oluşturulmuştur.

### ✅ Oluşturulan Testler

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

### 📋 Karşılanan Gereksinimler

✅ **En az 10 entegrasyon testi** (15 test oluşturuldu)  
✅ **API endpoint'leri testi** (HTTP request/response)  
✅ **Veritabanı işlemleri** (CRUD operasyonları)  
✅ **İlişkili kaynaklar arası işlemler** (User-Entry, Entry-Review, Entry-Tag)  
✅ **Hata durumları** (404, 400 testleri)  
✅ **Her HTTP metodu test edildi** (GET, POST, PUT, DELETE)  
✅ **Test veritabanı kullanımı** (InMemory Database ile test isolation)

### 🎯 Puanlama Kriterleri

- **API endpoint testlerinin kapsamlılığı:** 
  - Tüm HTTP metodları (GET, POST, PUT, DELETE) test edildi
  - Başarılı ve hata senaryoları kapsandı
  
- **Veritabanı entegrasyon testleri:** 
  - CRUD işlemleri kapsamlı şekilde test edildi
  - İlişkili tablolar arası işlemler test edildi
  
- **Test veri yönetimi (setup/teardown):** 
  - Her test için izole InMemory Database kullanıldı
  - SeedDatabase helper metodu ile veri yönetimi sağlandı



### 🔧 Teknik Detaylar

**Kullanılan Teknolojiler:**
- Microsoft.AspNetCore.Mvc.Testing (10.0.1)
- Microsoft.EntityFrameworkCore.InMemory (10.0.1)
- xUnit (2.9.3)

**Test Mimarisi:**
- `CustomWebApplicationFactory` sınıfı ile test ortamı konfigürasyonu
- InMemory Database ile test isolation
- Environment-based database configuration (Testing/Production)

**Test Dosyası:**
`Software_Test_App.Tests/IntegrationTests.cs`


Entegrasyon testlerini çalıştırmak için:

```bash
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

## Sistem Test Özeti

sistemTestleri.md dosyasındaki kurallara göre **8 kapsamlı uçtan uca sistem testi** oluşturulmuştur.

### ✅ Oluşturulan Test Senaryoları

1. **CompleteUserJourney_CreateUser_AddEntry_AddReview_AddTag_SystemTest**
   - Kullanıcı kaydı → Entry oluşturma → Review ekleme → Tag ekleme
   - Tam bir kullanıcı yolculuğunu simüle eder
   
2. **ContentManagement_List_View_Update_Delete_SystemTest**
   - İçerik listeleme → Detay görüntüleme → Güncelleme → Silme
   - İçerik yönetimi iş akışını test eder

3. **SearchWorkflow_CreateEntries_PerformSearch_ViewResults_SystemTest**
   - Entry oluşturma → Arama yapma → Sonuçları görüntüleme → Arama geçmişi
   - Arama fonksiyonalitesinin tam döngüsünü test eder

4. **MultiUserInteraction_MultipleUsers_CrossReviews_SystemTest**
   - Çoklu kullanıcı oluşturma → Her kullanıcı entry ekler → Çapraz review'lar
   - Birden fazla kullanıcının sistem üzerindeki etkileşimlerini test eder

5. **ComplexBusinessScenario_FullWorkflow_WithModifications_SystemTest**
   - Entry oluştur → Çoklu review → Tag ekleme → Güncelleme → Review silme
   - Karmaşık iş senaryolarını ve modifikasyonları test eder

6. **ErrorHandling_InvalidOperations_ProperErrorCodes_SystemTest**
   - Geçersiz verilerle işlemler → Doğru hata kodlarının dönmesi
   - Sistem hata yönetimini ve hata kodlarını test eder

7. **BulkDataProcessing_CreateMultiple_ListAndFilter_SystemTest**
   - Toplu kullanıcı oluşturma → Her kullanıcı için çoklu entry → Tag ekleme
   - Sistemin toplu veri işleme kabiliyetini test eder

8. **DataIntegrity_RelatedResources_ConsistencyCheck_SystemTest**
   - İlişkili kaynaklarla işlemler → Veri tutarlılığı kontrolü
   - Veri bütünlüğü ve ilişkisel tutarlılığı test eder

### 📋 Karşılanan Gereksinimler

✅ **En az 5 farklı senaryo** (8 test senaryosu oluşturuldu)  
✅ **Gerçek kullanım durumları simülasyonu** (Her senaryo gerçek kullanıcı davranışlarını yansıtır)  
✅ **Bağımsız test çalışması** (Her test kendi verilerini oluşturur ve bağımsızdır)  
✅ **Kompleks iş akışları** (Birden fazla kaynakla ilgili senaryolar test edildi)  
✅ **Uçtan uca test** (Sistemin bir bütün olarak çalışması doğrulandı)

### 🎯 Test Kapsamı

**Senaryo Tipleri:**
- **Kullanıcı Yolculukları:** Tam kullanıcı iş akışları (kayıt, içerik oluşturma, etkileşim)
- **CRUD İşlemleri:** Create, Read, Update, Delete operasyonlarının entegrasyonu
- **Arama ve Keşif:** Search API'sinin tam döngü testi
- **Çoklu Kullanıcı Etkileşimleri:** Birden fazla kullanıcının sistem kullanımı
- **Hata Senaryoları:** Geçersiz girişler ve hata yönetimi
- **Veri Bütünlüğü:** İlişkisel veri tutarlılığı ve bütünlük kontrolleri
- **Toplu İşlemler:** Çoklu kaynak oluşturma ve yönetimi

**Kapsanan İş Akışları:**
- User → Entry → Review → Tag (tam döngü)
- Entry CRUD operasyonları
- Search functionality (query + history)
- Multi-user interactions
- Complex business scenarios
- Error handling paths
- Bulk data operations
- Data integrity checks

### 🔧 Teknik Detaylar

**Test Özellikleri:**
- Her test senaryosu tamamen bağımsızdır
- Her test kendi test verilerini oluşturur
- InMemory Database ile test izolasyonu sağlanır
- Gerçek HTTP istekleri ile API testleri yapılır
- Tüm CRUD operasyonları ve ilişkiler test edilir

**Kullanılan Teknolojiler:**
- Microsoft.AspNetCore.Mvc.Testing (10.0.1)
- Microsoft.EntityFrameworkCore.InMemory (10.0.1)
- xUnit (2.9.3)
- HttpClient (JSON API testing)

**Test Dosyası:**
`Software_Test_App.Tests/SystemTests.cs`

Sistem testlerini çalıştırmak için:

```bash
dotnet test --filter "FullyQualifiedName~SystemTests"
```

Tüm testleri çalıştırmak için:

```bash
dotnet test
```