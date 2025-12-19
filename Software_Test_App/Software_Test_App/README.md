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
