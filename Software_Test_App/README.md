# Software Test App

Bu proje, Yazılım Kalite Güvence Testi dersi kapsamında geliştirilen bir API uygulamasıdır. Ürün yönetimi (CRUD) işlemlerini gerçekleştiren bir RESTful API sunar.

## Kullanılan Teknolojiler

- **Dil:** C# (.NET 10.0)
- **Veritabanı:** SQLite
- **API Dokümantasyonu:** Swagger UI
- **Test Çerçevesi:** xUnit

## Kurulum Talimatları

Projenin çalıştırılabilmesi için bilgisayarınızda .NET SDK yüklü olmalıdır.

1.  Repoyu klonlayın:
    ```bash
    git clone <repo-url>
    cd Software_Test_App
    ```

2.  Bağımlılıkları yükleyin:
    ```bash
    dotnet restore
    ```

3.  Veritabanını oluşturun:
    ```bash
    dotnet ef database update --project Software_Test_App/Software_Test_App.csproj
    ```

4.  Projeyi çalıştırın:
    ```bash
    dotnet run --project Software_Test_App/Software_Test_App.csproj
    ```

## API Endpointleri

| Metot  | Endpoint           | Açıklama                   |
| :----- | :----------------- | :------------------------- |
| GET    | `/api/products`    | Tüm ürünleri listeler      |
| GET    | `/api/products/{id}`| ID'ye göre ürün getirir    |
| POST   | `/api/products`    | Yeni ürün ekler            |
| PUT    | `/api/products/{id}`| Mevcut ürünü günceller     |
| DELETE | `/api/products/{id}`| Ürünü siler                |

## Swagger / OpenAPI Dokümantasyonu

Uygulama çalışırken Swagger UI arayüzüne aşağıdaki adresten erişebilirsiniz:

- **URL:** `http://localhost:5000/swagger/index.html` (veya port numarasına göre değişebilir, örn: `https://localhost:7198/swagger/index.html`)

Swagger arayüzü üzerinden tüm API endpointlerini interaktif olarak test edebilirsiniz.

## Test Çalıştırma Komutları

Testleri çalıştırmak için proje kök dizininde aşağıdaki komutu kullanın:

```bash
dotnet test
```

## CI/CD Durumu

![Build Status](https://github.com/username/repo/actions/workflows/dotnet.yml/badge.svg)
![Code Coverage](https://codecov.io/gh/username/repo/branch/main/graph/badge.svg)

