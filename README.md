# 💱 Currency Exchange Microservices Application

Bu proje, Türkiye Cumhuriyet Merkez Bankası (TCMB) verilerini kullanarak güncel döviz kurlarını çeken, saklayan, önbellekleyen ve görselleştiren çok katmanlı bir mikroservis mimarisidir. Sistem; veri yönetimi, iş mantığı ve kullanıcı arayüzü olarak birbirinden izole edilmiş bileşenlerin Docker üzerinde orkestrasyonu ile çalışır.

## 🚀 Proje Mimarisi ve Katmanlar

Uygulama, bağımsız olarak ölçeklenebilen 3 temel servis ve 2 veri bileşeninden oluşmaktadır:

*   **Currency.DataApi (Veri Entegrasyon Katmanı):** TCMB XML servisi ile haberleşerek günlük kur verilerini (USD, EUR, GBP, JPY vb.) çeker ve PostgreSQL veritabanına senkronize eder.
*   **Currency.BusinessApi (İş ve Önbellek Katmanı):** Web istemcisinden gelen talepleri karşılar. Veritabanı yükünü hafifletmek için yüksek performanslı veri okuma işlemini Redis önbelleği (caching) üzerinden gerçekleştirir. Önbellekte veri yoksa (cache miss) veritabanına gider, veriyi alır, Redis'e yazar ve istemciye döner.
*   **Currency.Web (Kullanıcı Arayüzü):** Kullanıcıların kur kodlarına göre arama yapabildiği, jQuery (Ajax) ile Business API'den verileri asenkron çektiği ve **D3.js** kullanarak kur grafiklerini çizdiği sunum katmanıdır.
*   **PostgreSQL:** Kalıcı veri depolama birimi. Entity Framework Core Code-First yaklaşımı ile otomatik migrasyon kullanılarak yönetilmektedir.
*   **Redis:** Yüksek performanslı in-memory veri yapısı deposu. Kur sorgularında yanıt süresini milisaniyelere düşürmek için kullanılmıştır.

## 🛠 Kullanılan Teknolojiler

*   **Backend:** C#, .NET 8 Web API, Entity Framework Core
*   **Veritabanı & Önbellek:** PostgreSQL, Redis
*   **Konteynerleştirme (Orchestration):** Docker, Docker Compose
*   **Frontend:** HTML5, CSS3, Bootstrap, jQuery, Ajax
*   **Veri Görselleştirme:** D3.js

## ⚙️ Kurulum ve Çalıştırma

Projeyi lokal ortamınızda tek bir komutla ayağa kaldırabilirsiniz. Tüm veritabanı kurulumları ve API yapılandırmaları Docker üzerinden otomatik gerçekleşir.

### Gereksinimler
*   Docker Desktop
*   Git

### Adımlar

1.  **Projeyi Klonlayın:**
    ```bash
    git clone [https://github.com/erenay44/MerkezBankasi_Kur.git]
    cd MerkezBankasi_Kur
    ```

2.  **Sistemi Ayağa Kaldırın:**
    Docker Compose kullanarak tüm mikroservisleri başlatın:
    ```bash
    docker-compose up --build -d
    ```

3.  **TCMB Verilerini Senkronize Edin (İlk Kurulum):**
    Veritabanını doldurmak için Data API Swagger arayüzüne gidin ve `/api/Currency/sync` POST metodunu çalıştırın.
    *   **Data API Swagger:** `http://localhost:5002/swagger/index.html`

4.  **Uygulamayı Kullanın:**
    Veriler çekildikten sonra web arayüzüne giderek döviz kurlarını anlık olarak listeleyebilir ve D3.js ile çizilmiş grafiklerini inceleyebilirsiniz.
    *   **Web Arayüzü:** `http://localhost:5000`
    *   *(Not: Business API `5001` portundan çalışmaktadır).*

## 👨‍💻 Geliştirici

**Erenay Ceyran**
Computer Engineering | Backend & Full-Stack Developer
[LinkedIn](https://www.linkedin.com/in/erenayceyran) | [GitHub](github.com/erenay44)
