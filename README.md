# 📒 MyNotes / Tariflerim Projesi

Modern web teknolojileri kullanılarak geliştirilmiş, kullanıcıların notlarını veya tariflerini yönetebileceği bir **ASP.NET MVC** uygulamasıdır.

## 🚀 Proje Hakkında

Bu proje, kullanıcıların:
- Not / tarif oluşturmasını
- Güncellemesini
- Silmesini
- Beğeni (Like) işlemleri yapmasını
- Kategorilere göre içerikleri filtrelemesini

sağlayan full-stack bir web uygulamasıdır.

Aynı zamanda proje, **katmanlı mimari**, **Entity Framework** kullanımı ve **authentication/authorization** gibi önemli backend konseptlerini öğrenmek amacıyla geliştirilmiştir.

---

## 🛠️ Kullanılan Teknolojiler

- ASP.NET MVC (.NET Framework 4.6.2)
- Entity Framework (Code First)
- MSSQL Server
- Bootstrap 5
- HTML / CSS / JavaScript
- Razor View Engine

---

## 🧱 Mimari Yapı

Proje, sürdürülebilir ve genişletilebilir olması için katmanlı mimari ile geliştirilmiştir:

- **Entity Layer** → Veritabanı modelleri
- **Data Access Layer (Repository Pattern)** → Veri erişim işlemleri
- **Business Layer (Manager Classes)** → İş kuralları
- **Presentation Layer (MVC)** → UI ve Controller yapısı

### 📌 Önemli Mimari Kararlar

- Her request için yeni **DbContext** oluşturulmaktadır (Best Practice)
- Static DbContext kullanımından kaçınılmıştır
- Foreign Key ilişkileri ID üzerinden yönetilmektedir
- Like/Unlike işlemleri atomik şekilde ele alınmıştır
- EF Cascade Delete davranışı kontrollü şekilde yapılandırılmıştır

---

## 🔐 Kimlik Doğrulama & Yetkilendirme

- Kullanıcı kayıt ve giriş sistemi bulunmaktadır
- Session bazlı authentication kullanılmıştır
- Admin paneli için yetkilendirme uygulanmıştır

---

## ✨ Özellikler

- 📝 Not / Tarif ekleme, silme, güncelleme
- ❤️ Like / Unlike sistemi
- 🗂️ Kategori bazlı filtreleme
- 👤 Kullanıcı profili yönetimi
- 🔐 Login / Register sistemi
- 🛠️ Admin paneli

---

## 🖼️ UI / UX

- Bootstrap 5 ile responsive tasarım
- Mobil uyumlu navigation ve kategori yapısı
- Collapse / toggle kullanımı ile dinamik listeleme

---

## ⚙️ Kurulum

Projeyi lokal ortamda çalıştırmak için:

```bash
git clone https://github.com/kullaniciadi/proje-adi.git
```

1. Visual Studio ile aç
2. MSSQL bağlantı ayarlarını `Web.config` üzerinden güncelle
3. Package Manager Console'da:

```powershell
Update-Database
```

4. Uygulamayı çalıştır

---

## 📂 Veritabanı

- Code First yaklaşımı kullanılmıştır
- Migration yapısı aktiftir
- İlişkiler Fluent API ile yapılandırılmıştır

---

## 🧪 Gelecek Geliştirmeler

- Unit & Integration testler
- JWT tabanlı authentication
- API katmanı eklenmesi
- React / modern frontend entegrasyonu
- Cloud deployment (Azure / Docker)

---

## 📌 Amaç

Bu proje:
- .NET MVC mimarisini derinlemesine öğrenmek
- Backend best practice'lerini uygulamak
- Gerçek dünya senaryolarına yakın bir sistem geliştirmek

amacıyla geliştirilmiştir.

---

## 👨‍💻 Geliştirici

**Abbas Çoban**

---

## ⭐ Katkı

Projeyi beğendiysen ⭐ bırakmayı unutma!
