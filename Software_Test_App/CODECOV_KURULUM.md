# 📊 Codecov Kurulum ve Kullanım Rehberi - .NET 8.0

## ✅ Yapılan Değişiklikler

### 1. .NET Versiyonu Güncellendi
- ✅ .NET 10.0 → .NET 8.0 (LTS)
- ✅ Tüm paket versiyonları .NET 8.0 uyumlu hale getirildi
- ✅ Build ve testler başarıyla çalışıyor

### 2. Coverage Konfigürasyonu
- ✅ `coverlet.msbuild` paketi eklendi
- ✅ OpenCover format desteği eklendi
- ✅ Test sonuçları: **73.87% Line Coverage** ✓

### 3. GitHub Actions Workflow Eklendi
- ✅ `.github/workflows/dotnet.yml` oluşturuldu
- ✅ Otomatik build, test ve coverage raporu
- ✅ Codecov entegrasyonu hazır

## 🚀 Hızlı Başlangıç

### Yerel Olarak Coverage Raporu Üretme

```powershell
# Testleri coverage ile çalıştır
cd Software_Test_App.Tests
dotnet test --configuration Release /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=./coverage/

# Coverage dosyası oluşturuldu:
# Software_Test_App.Tests/coverage/coverage.opencover.xml
```

**Sonuç:**
```
+-------------------+--------+--------+--------+
| Module            | Line   | Branch | Method |
+-------------------+--------+--------+--------+
| Software_Test_App | 73.87% | 50%    | 86.88% |
+-------------------+--------+--------+--------+
```

### Coverage Raporu Görüntüleme (Opsiyonel)

```powershell
# ReportGenerator global tool'u yükle (ilk kez)
dotnet tool install -g dotnet-reportgenerator-globaltool

# HTML raporu oluştur
reportgenerator -reports:"Software_Test_App.Tests/coverage/coverage.opencover.xml" -targetdir:"coveragereport" -reporttypes:Html

# Raporu tarayıcıda aç
start coveragereport/index.html
```

## 🔐 Codecov GitHub Entegrasyonu

### Adım 1: Codecov Hesabı Oluştur

1. **https://codecov.io/** adresine git
2. **"Sign up with GitHub"** seçeneğini kullan
3. GitHub hesabınla giriş yap ve yetkilendir

### Adım 2: Repository'yi Ekle

1. Codecov Dashboard'da **"Add new repository"** tıkla
2. Repository'nizi listeden seç ve **"Setup repo"** tıkla
3. **Codecov Token'ı** kopyala

### Adım 3: GitHub Secret Ekle

1. GitHub repository ayarlarına git:
   ```
   https://github.com/KULLANICI_ADINIZ/Software_Test_App/settings/secrets/actions
   ```

2. **"New repository secret"** tıkla
3. **Name:** `CODECOV_TOKEN`
4. **Secret:** (Codecov'dan kopyaladığın token)
5. **"Add secret"** tıkla

### Adım 4: Kodu Push Et

```bash
git add .
git commit -m "Migrate to .NET 8.0 and setup Codecov"
git push origin main
```

### Adım 5: Sonucu Kontrol Et

1. **GitHub Actions:**
   ```
   https://github.com/KULLANICI_ADINIZ/Software_Test_App/actions
   ```
   - Workflow'un başarıyla tamamlandığını kontrol et

2. **Codecov Dashboard:**
   ```
   https://codecov.io/gh/KULLANICI_ADINIZ/Software_Test_App
   ```
   - Coverage raporunu görüntüle

## 📊 Mevcut Coverage Durumu

```
Total Coverage: 73.87%
├── Line Coverage:   73.87%
├── Branch Coverage: 50.00%
└── Method Coverage: 86.88%
```

### Test İstatistikleri
- ✅ **44 test** başarıyla geçti
- ✅ **0 başarısız** test
- ✅ **0 atlanan** test
- ⏱️ Toplam süre: ~1 saniye

### Hariç Tutulanlar (codecov.yml)
- Migration dosyaları
- Test dosyaları
- bin/obj klasörleri
- Designer dosyaları
- Program.cs (minimal API bootstrap)

## 🛠️ Sorun Giderme

### 1. Build Hatası

```powershell
# Clean ve restore
dotnet clean
dotnet restore
dotnet build --configuration Release
```

### 2. Coverage Dosyası Oluşmuyor

```powershell
# Paketlerin yüklü olduğundan emin ol
dotnet restore Software_Test_App.Tests/Software_Test_App.Tests.csproj

# Verbose mode ile test çalıştır
dotnet test --verbosity detailed /p:CollectCoverage=true
```

### 3. Codecov Upload Hatası

**Public Repository İçin:**
- Token opsiyoneldir, olmasa da çalışır
- GitHub Actions'da `fail_ci_if_error: false` ayarlandı

**Private Repository İçin:**
- `CODECOV_TOKEN` secret'ının doğru olduğunu kontrol et
- Token'ı Codecov'dan tekrar kopyalayıp ekle

### 4. GitHub Actions Çalışmıyor

```yaml
# .github/workflows/dotnet.yml kontrol et
# Dosyanın doğru yerde olduğundan emin ol
# Branch ismini kontrol et (main veya master)
```

## 📈 Coverage'ı Artırma Önerileri

### Eksik Test Alanları
1. **Controller Exception Handling**
   - Hata durumları için testler ekle
   - Validation failure testleri

2. **Model Validation**
   - Edge case'ler için testler
   - Null/empty değer testleri

3. **Branch Coverage**
   - If/else dalları için testler
   - Switch case'ler için testler

### Örnek Test Ekleme

```csharp
[Fact]
public async Task GetEntry_InvalidId_ReturnsNotFound()
{
    // Arrange
    var invalidId = 999;
    
    // Act
    var result = await _controller.GetEntry(invalidId);
    
    // Assert
    Assert.IsType<NotFoundResult>(result.Result);
}
```

## 📋 CI/CD Pipeline

### GitHub Actions Workflow Özellikleri
- ✅ .NET 8.0 SDK kullanımı
- ✅ Otomatik restore ve build
- ✅ Test execution
- ✅ Coverage raporu oluşturma
- ✅ Codecov'a otomatik upload
- ✅ Hata durumunda pipeline devam eder

### Trigger Koşulları
- `push` to main/master branch
- `pull_request` to main/master branch

## 🎯 Hedef Coverage Oranları

```yaml
Minimum (Mevcut):
├── Line:   73.87% ✓
├── Branch: 50.00% ⚠️
└── Method: 86.88% ✓

İdeal:
├── Line:   80%+
├── Branch: 70%+
└── Method: 90%+
```

## 📚 Ek Kaynaklar

- **Codecov Docs:** https://docs.codecov.com/
- **Coverlet GitHub:** https://github.com/coverlet-coverage/coverlet
- **GitHub Actions:** https://docs.github.com/en/actions
- **.NET Testing:** https://learn.microsoft.com/en-us/dotnet/core/testing/

## 🎉 Özet

✅ Proje başarıyla .NET 8.0'a geçirildi  
✅ Coverage sistemi kuruldu ve çalışıyor  
✅ GitHub Actions entegrasyonu hazır  
✅ Codecov için tüm altyapı mevcut  
✅ %73.87 coverage oranı elde edildi  

**Şimdi yapman gereken:**
1. GitHub'a kodu push et
2. Codecov'a repository ekle
3. Token'ı GitHub Secrets'a ekle
4. İlk coverage raporunu görüntüle! 🚀

