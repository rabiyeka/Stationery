using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stationery.Models;

namespace Stationery.Data;

public class StationeryDbContext : IdentityDbContext<StationeryUser>
{
    public StationeryDbContext(DbContextOptions<StationeryDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Product>()
            .HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(10, 2);

        builder.Entity<Brand>()
            .Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Entity<Brand>()
            .HasIndex(b => b.Name)
            .IsUnique();

        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Cart>()
            .HasIndex(c => c.UserId)
            .IsUnique();

        builder.Entity<CartItem>()
            .HasOne(i => i.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(i => i.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CartItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CartItem>()
            .HasIndex(i => new { i.CartId, i.ProductId })
            .IsUnique();

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(10, 2);

        builder.Entity<Order>()
            .Property(o => o.OrderStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Entity<OrderItem>()
            .HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderItem>()
            .Property(i => i.UnitPrice)
            .HasPrecision(10, 2);

        SeedData(builder);

    }

    private static void SeedData(ModelBuilder builder)
    {
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Kalemler", Description = "Uçlu, tükenmez, kurşun vb." },
            new Category { Id = 2, Name = "Defterler", Description = "Spiralli, dikişli, kareli ve çizgili defterler" },
            new Category { Id = 3, Name = "Boya Kalemleri", Description = "Kuru, sulu, pastel boyalar" },
            new Category { Id = 4, Name = "Test Kitapları", Description = "LGS, YKS, KPSS hazırlık ve soru bankaları" },
            new Category { Id = 5, Name = "Ofis & Masaüstü", Description = "Zımba, bant, makas, ataş" },
            new Category { Id = 6, Name = "Silgi & Kalemtıraş", Description = "Sınav silgileri, mekanik kalemtıraşlar" },
            new Category { Id = 7, Name = "Kağıt Ürünleri", Description = "Fotokopi kağıtları, notluklar" },
            new Category { Id = 8, Name = "Çanta & Beslenme", Description = "Okul çantaları ve beslenme kapları" },
            new Category { Id = 9, Name = "Dosyalama", Description = "Klasörler, poşet dosyalar" }
        );


        builder.Entity<Brand>().HasData(
            new Brand { Id = 1, Name = "Faber-Castell" },
            new Brand { Id = 2, Name = "Rotring" },
            new Brand { Id = 3, Name = "Stabilo" },
            new Brand { Id = 4, Name = "Adel" },
            new Brand { Id = 5, Name = "Bic" },
            new Brand { Id = 6, Name = "Gıpta" },
            new Brand { Id = 7, Name = "Moleskine" },
            new Brand { Id = 8, Name = "Keskin Color" },
            new Brand { Id = 9, Name = "Carioca" },
            new Brand { Id = 10, Name = "Limit Yayınları" },
            new Brand { Id = 11, Name = "Bilgi Sarmal" },
            new Brand { Id = 12, Name = "3D Yayınları" },
            new Brand { Id = 13, Name = "Çap Yayınları" },
            new Brand { Id = 14, Name = "Hız Yayınları" },
            new Brand { Id = 15, Name = "Palme Yayınevi" },
            new Brand { Id = 16, Name = "Tonguç Akademi" },
            new Brand { Id = 17, Name = "MAS" },
            new Brand { Id = 18, Name = "Maped" },
            new Brand { Id = 19, Name = "Pritt" },
            new Brand { Id = 20, Name = "Milan" },
            new Brand { Id = 21, Name = "Navigator" },
            new Brand { Id = 22, Name = "Mopak" },
            new Brand { Id = 23, Name = "Ve-Ge" },
            new Brand { Id = 24, Name = "Eastpak" },
            new Brand { Id = 25, Name = "Noki" },
            new Brand { Id = 26, Name = "Esselte" }
        );
        // 3. ÜRÜNLER (Görselleri Placehold.co ile güncellenmiş tam liste)
        builder.Entity<Product>().HasData(
            // === 1. KALEMLER ===
            new Product { Id = 1, Name = "Faber-Castell Grip 2011 0.7mm", Description = "Versatil Kalem", Price = 250m, StockQuantity = 100, ImageUrl = "https://placehold.co/400x400?text=Faber+Grip+2011", CategoryId = 1, BrandId = 1 },
            new Product { Id = 2, Name = "Faber-Castell Tri-Click 0.5mm", Description = "Uçlu Kalem", Price = 45m, StockQuantity = 200, ImageUrl = "https://placehold.co/400x400?text=Faber+Tri+Click", CategoryId = 1, BrandId = 1 },
            new Product { Id = 3, Name = "Rotring Tikky 0.7mm Bordo", Description = "Mekanik Kalem", Price = 185m, StockQuantity = 150, ImageUrl = "https://placehold.co/400x400?text=Rotring+Tikky", CategoryId = 1, BrandId = 2 },
            new Product { Id = 4, Name = "Rotring 500 0.5mm Siyah", Description = "Profesyonel Çizim Kalemi", Price = 450m, StockQuantity = 50, ImageUrl = "https://placehold.co/400x400?text=Rotring+500", CategoryId = 1, BrandId = 2 },
            new Product { Id = 5, Name = "Stabilo Boss Original Sarı", Description = "Fosforlu Kalem", Price = 55m, StockQuantity = 300, ImageUrl = "https://placehold.co/400x400?text=Stabilo+Boss", CategoryId = 1, BrandId = 3 },
            new Product { Id = 6, Name = "Stabilo Point 88 10'lu Set", Description = "İnce Uçlu Keçeli", Price = 320m, StockQuantity = 80, ImageUrl = "https://placehold.co/400x400?text=Stabilo+Point+88", CategoryId = 1, BrandId = 3 },
            new Product { Id = 7, Name = "Adel Blackline Kurşun Kalem", Description = "Sınav Kalemi", Price = 15m, StockQuantity = 500, ImageUrl = "https://placehold.co/400x400?text=Adel+Kursun+Kalem", CategoryId = 1, BrandId = 4 },
            new Product { Id = 8, Name = "Adel 0.7mm Versatil", Description = "Öğrenci Uçlu Kalem", Price = 35m, StockQuantity = 250, ImageUrl = "https://placehold.co/400x400?text=Adel+Versatil", CategoryId = 1, BrandId = 4 },
            new Product { Id = 9, Name = "Bic Cristal Mavi Tükenmez 5'li", Description = "Klasik Tükenmez", Price = 45m, StockQuantity = 400, ImageUrl = "https://placehold.co/400x400?text=Bic+Cristal", CategoryId = 1, BrandId = 5 },
            new Product { Id = 10, Name = "Bic 4 Renk Tükenmez Kalem", Description = "Çoklu Tükenmez", Price = 85m, StockQuantity = 120, ImageUrl = "https://placehold.co/400x400?text=Bic+4+Renk", CategoryId = 1, BrandId = 5 },

            // === 2. DEFTERLER ===
            new Product { Id = 11, Name = "Faber-Castell A4 Çizgili 96 Yp", Description = "Spiralli Defter", Price = 85m, StockQuantity = 200, ImageUrl = "https://placehold.co/400x400?text=Faber+A4+Defter", CategoryId = 2, BrandId = 1 },
            new Product { Id = 12, Name = "Faber-Castell A5 Kareli 80 Yp", Description = "Sert Kapak", Price = 65m, StockQuantity = 150, ImageUrl = "https://placehold.co/400x400?text=Faber+A5+Defter", CategoryId = 2, BrandId = 1 },
            new Product { Id = 13, Name = "Gıpta A4 120 Yaprak 5 Bölmeli", Description = "Seperatörlü Defter", Price = 185m, StockQuantity = 100, ImageUrl = "https://placehold.co/400x400?text=Gipta+A4+Defter", CategoryId = 2, BrandId = 6 },
            new Product { Id = 14, Name = "Gıpta A5 Pastel Notluk", Description = "Çizgisiz Not Defteri", Price = 55m, StockQuantity = 300, ImageUrl = "https://placehold.co/400x400?text=Gipta+Notluk", CategoryId = 2, BrandId = 6 },
            new Product { Id = 15, Name = "Adel İlkokul Güzel Yazı Defteri", Description = "Kılavuz Çizgili", Price = 25m, StockQuantity = 400, ImageUrl = "https://placehold.co/400x400?text=Adel+Yazi+Defteri", CategoryId = 2, BrandId = 4 },
            new Product { Id = 16, Name = "Moleskine Classic Pocket Siyah", Description = "Deri Kapaklı Ajanda", Price = 850m, StockQuantity = 30, ImageUrl = "https://placehold.co/400x400?text=Moleskine+Classic", CategoryId = 2, BrandId = 7 },
            new Product { Id = 17, Name = "Moleskine Cahier Journal 3'lü", Description = "İnce Not Defteri", Price = 450m, StockQuantity = 45, ImageUrl = "https://placehold.co/400x400?text=Moleskine+Cahier", CategoryId = 2, BrandId = 7 },
            new Product { Id = 18, Name = "Keskin Color A4 Spiralli Resim Defteri", Description = "120gr Kağıt", Price = 95m, StockQuantity = 180, ImageUrl = "https://placehold.co/400x400?text=Keskin+Color+Resim", CategoryId = 2, BrandId = 8 },
            new Product { Id = 19, Name = "Keskin Color Müzik Defteri", Description = "Dikişli", Price = 35m, StockQuantity = 200, ImageUrl = "https://placehold.co/400x400?text=Keskin+Color+Muzik", CategoryId = 2, BrandId = 8 },

            // === 3. BOYA KALEMLERİ ===
            new Product { Id = 20, Name = "Faber-Castell 24'lü Kuru Boya", Description = "Karton Kutu", Price = 275m, StockQuantity = 120, ImageUrl = "https://placehold.co/400x400?text=Faber+24lu+Kuru+Boya", CategoryId = 3, BrandId = 1 },
            new Product { Id = 21, Name = "Faber-Castell 12'li Sulu Boya", Description = "Fırça Hediyeli", Price = 145m, StockQuantity = 150, ImageUrl = "https://placehold.co/400x400?text=Faber+12li+Sulu+Boya", CategoryId = 3, BrandId = 1 },
            new Product { Id = 22, Name = "Adel 12'li Yağlı Pastel Boya", Description = "Çanta Boy", Price = 85m, StockQuantity = 200, ImageUrl = "https://placehold.co/400x400?text=Adel+Pastel+Boya", CategoryId = 3, BrandId = 4 },
            new Product { Id = 23, Name = "Adel 12'li Kuru Boya", Description = "Yarım Boy", Price = 65m, StockQuantity = 250, ImageUrl = "https://placehold.co/400x400?text=Adel+Kuru+Boya", CategoryId = 3, BrandId = 4 },
            new Product { Id = 24, Name = "Stabilo Woody 3 in 1 10'lu", Description = "Bebek/Çocuk Boyası", Price = 650m, StockQuantity = 40, ImageUrl = "https://placehold.co/400x400?text=Stabilo+Woody", CategoryId = 3, BrandId = 3 },
            new Product { Id = 25, Name = "Carioca Joy 12'li Keçeli Kalem", Description = "Yıkanabilir", Price = 125m, StockQuantity = 180, ImageUrl = "https://placehold.co/400x400?text=Carioca+Joy", CategoryId = 3, BrandId = 9 },
            new Product { Id = 26, Name = "Carioca Jumbo 6'lı Keçeli", Description = "Kalın Uçlu", Price = 110m, StockQuantity = 90, ImageUrl = "https://placehold.co/400x400?text=Carioca+Jumbo", CategoryId = 3, BrandId = 9 },

            // === 4. TEST KİTAPLARI ===
            new Product { Id = 27, Name = "Limit TYT Türkçe Soru Bankası", Description = "Yeni Müfredat", Price = 240m, StockQuantity = 300, ImageUrl = "https://placehold.co/400x400?text=Limit+TYT+Turkce", CategoryId = 4, BrandId = 10 },
            new Product { Id = 28, Name = "Limit AYT Edebiyat Soru Bankası", Description = "Çözümlü", Price = 260m, StockQuantity = 250, ImageUrl = "https://placehold.co/400x400?text=Limit+AYT+Edebiyat", CategoryId = 4, BrandId = 10 },
            new Product { Id = 29, Name = "Bilgi Sarmal TYT Matematik", Description = "Video Çözümlü", Price = 295m, StockQuantity = 400, ImageUrl = "https://placehold.co/400x400?text=Sarmal+TYT+Mat", CategoryId = 4, BrandId = 11 },
            new Product { Id = 30, Name = "Bilgi Sarmal 11. Sınıf Fizik", Description = "Soru Bankası", Price = 220m, StockQuantity = 150, ImageUrl = "https://placehold.co/400x400?text=Sarmal+11+Fizik", CategoryId = 4, BrandId = 11 },
            new Product { Id = 31, Name = "3D TYT Matematik Simülasyon", Description = "Zor Seviye", Price = 310m, StockQuantity = 350, ImageUrl = "https://placehold.co/400x400?text=3D+TYT+Matematik", CategoryId = 4, BrandId = 12 },
            new Product { Id = 32, Name = "3D AYT Geometri", Description = "Tamamı Çözümlü", Price = 280m, StockQuantity = 200, ImageUrl = "https://placehold.co/400x400?text=3D+AYT+Geometri", CategoryId = 4, BrandId = 12 },
            new Product { Id = 33, Name = "Çap TYT Fizik Fasikülleri", Description = "Set Halinde", Price = 350m, StockQuantity = 120, ImageUrl = "https://placehold.co/400x400?text=Cap+TYT+Fizik", CategoryId = 4, BrandId = 13 },
            new Product { Id = 34, Name = "Hız Yayınları 8. Sınıf LGS Paragraf", Description = "Hızlandırılmış", Price = 180m, StockQuantity = 500, ImageUrl = "https://placehold.co/400x400?text=Hiz+LGS+Paragraf", CategoryId = 4, BrandId = 14 },
            new Product { Id = 35, Name = "Hız Yayınları 8. Sınıf LGS Matematik", Description = "Yeni Nesil", Price = 195m, StockQuantity = 450, ImageUrl = "https://placehold.co/400x400?text=Hiz+LGS+Matematik", CategoryId = 4, BrandId = 14 },
            new Product { Id = 36, Name = "Palme TYT Kimya Soru Bankası", Description = "Klasikleşmiş", Price = 250m, StockQuantity = 220, ImageUrl = "https://placehold.co/400x400?text=Palme+TYT+Kimya", CategoryId = 4, BrandId = 15 },
            new Product { Id = 37, Name = "Palme 10. Sınıf Biyoloji", Description = "Konu Anlatımlı", Price = 270m, StockQuantity = 140, ImageUrl = "https://placehold.co/400x400?text=Palme+10+Biyoloji", CategoryId = 4, BrandId = 15 },
            new Product { Id = 38, Name = "Tonguç 8. Sınıf Dinamo Matematik", Description = "LGS Hazırlık", Price = 210m, StockQuantity = 600, ImageUrl = "https://placehold.co/400x400?text=Tonguc+8+Matematik", CategoryId = 4, BrandId = 16 },
            new Product { Id = 39, Name = "Tonguç TYT Paragrafiks", Description = "Taktikli Soru Bankası", Price = 175m, StockQuantity = 450, ImageUrl = "https://placehold.co/400x400?text=Tonguc+Paragrafiks", CategoryId = 4, BrandId = 16 },

            // === 5. OFİS & MASAÜSTÜ ===
            new Product { Id = 40, Name = "MAS Masaüstü Organizer Set Siyah", Description = "Metal Fileli", Price = 240m, StockQuantity = 80, ImageUrl = "https://placehold.co/400x400?text=MAS+Organizer", CategoryId = 5, BrandId = 17 },
            new Product { Id = 41, Name = "MAS No:10 Zımba Teli 1000'li", Description = "Yedek Tel", Price = 25m, StockQuantity = 800, ImageUrl = "https://placehold.co/400x400?text=MAS+Zimba+Teli", CategoryId = 5, BrandId = 17 },
            new Product { Id = 42, Name = "Maped 21cm Ofis Makası", Description = "Paslanmaz Çelik", Price = 75m, StockQuantity = 150, ImageUrl = "https://placehold.co/400x400?text=Maped+Makas", CategoryId = 5, BrandId = 18 },
            new Product { Id = 43, Name = "Maped Ergonomik Delgeç", Description = "20 Sayfa Kapasite", Price = 165m, StockQuantity = 90, ImageUrl = "https://placehold.co/400x400?text=Maped+Delgec", CategoryId = 5, BrandId = 18 },
            new Product { Id = 44, Name = "Pritt Stick Yapıştırıcı 43g", Description = "Büyük Boy", Price = 65m, StockQuantity = 300, ImageUrl = "https://placehold.co/400x400?text=Pritt+Stick", CategoryId = 5, BrandId = 19 },
            new Product { Id = 45, Name = "Pritt Sıvı Yapıştırıcı", Description = "Çok Amaçlı", Price = 55m, StockQuantity = 250, ImageUrl = "https://placehold.co/400x400?text=Pritt+Sivi", CategoryId = 5, BrandId = 19 },
            new Product { Id = 46, Name = "Faber-Castell Tack-It Hamur Yapıştırıcı", Description = "İz Bırakmaz", Price = 45m, StockQuantity = 400, ImageUrl = "https://placehold.co/400x400?text=Faber+Tack-It", CategoryId = 5, BrandId = 1 },
            new Product { Id = 47, Name = "Bic Beyaz Tahta Kalemi 4'lü", Description = "Silinebilir", Price = 120m, StockQuantity = 180, ImageUrl = "https://placehold.co/400x400?text=Bic+Tahta+Kalemi", CategoryId = 5, BrandId = 5 },

            // === 6. SİLGİ & KALEMTIRAŞ ===
            new Product { Id = 48, Name = "Faber-Castell Dust-Free Sınav Silgisi", Description = "Toz Bırakmaz", Price = 25m, StockQuantity = 1000, ImageUrl = "https://placehold.co/400x400?text=Faber+Dust-Free", CategoryId = 6, BrandId = 1 },
            new Product { Id = 49, Name = "Faber-Castell Çift Delikli Metal Kalemtıraş", Description = "Haznesiz", Price = 45m, StockQuantity = 300, ImageUrl = "https://placehold.co/400x400?text=Faber+Metal+Kalemtiras", CategoryId = 6, BrandId = 1 },
            new Product { Id = 50, Name = "Rotring Tikky Silgi 2'li", Description = "Yumuşak Silgi", Price = 35m, StockQuantity = 400, ImageUrl = "https://placehold.co/400x400?text=Rotring+Silgi", CategoryId = 6, BrandId = 2 },
            new Product { Id = 51, Name = "Stabilo Legacy Silgi", Description = "Renkli", Price = 20m, StockQuantity = 500, ImageUrl = "https://placehold.co/400x400?text=Stabilo+Silgi", CategoryId = 6, BrandId = 3 },
            new Product { Id = 52, Name = "Maped Shaker Hazneli Kalemtıraş", Description = "Dökülmez", Price = 58m, StockQuantity = 200, ImageUrl = "https://placehold.co/400x400?text=Maped+Shaker", CategoryId = 6, BrandId = 18 },
            new Product { Id = 53, Name = "Maped Salyangoz Silgi", Description = "Korumalı", Price = 35m, StockQuantity = 150, ImageUrl = "https://placehold.co/400x400?text=Maped+Salyangoz", CategoryId = 6, BrandId = 18 },
            new Product { Id = 54, Name = "Milan 430 Klasik Kauçuk Silgi", Description = "İspanyol Yapımı", Price = 15m, StockQuantity = 600, ImageUrl = "https://placehold.co/400x400?text=Milan+430+Silgi", CategoryId = 6, BrandId = 20 },
            new Product { Id = 55, Name = "Milan Capsule Silgili Kalemtıraş", Description = "2'si 1 Arada", Price = 85m, StockQuantity = 250, ImageUrl = "https://placehold.co/400x400?text=Milan+Capsule", CategoryId = 6, BrandId = 20 },

            // === 7. KAĞIT ÜRÜNLERİ ===
            new Product { Id = 56, Name = "Navigator A4 80g Fotokopi Kağıdı", Description = "500'lü Paket", Price = 165m, StockQuantity = 500, ImageUrl = "https://placehold.co/400x400?text=Navigator+A4", CategoryId = 7, BrandId = 21 },
            new Product { Id = 57, Name = "Gıpta Renkli A4 Elişi Kağıdı", Description = "10 Renk", Price = 45m, StockQuantity = 300, ImageUrl = "https://placehold.co/400x400?text=Gipta+Elisi+Kagidi", CategoryId = 7, BrandId = 6 },
            new Product { Id = 58, Name = "Gıpta Şeffaf Yapışkanlı Notluk", Description = "Su Geçirmez", Price = 35m, StockQuantity = 400, ImageUrl = "https://placehold.co/400x400?text=Gipta+Yapiskanli+Notluk", CategoryId = 7, BrandId = 6 },
            new Product { Id = 59, Name = "Mopak A4 80g Kutu (5 Paket)", Description = "Koli Bazlı Fotokopi Kağıdı", Price = 800m, StockQuantity = 50, ImageUrl = "https://placehold.co/400x400?text=Mopak+A4+Kutu", CategoryId = 7, BrandId = 22 },
            new Product { Id = 60, Name = "Mopak Resim Kağıdı 25x35", Description = "20'li Paket", Price = 40m, StockQuantity = 200, ImageUrl = "https://placehold.co/400x400?text=Mopak+Resim+Kagidi", CategoryId = 7, BrandId = 22 },
            new Product { Id = 61, Name = "Ve-Ge Sarı Küp Not", Description = "75x75 Yapışkanlı", Price = 25m, StockQuantity = 600, ImageUrl = "https://placehold.co/400x400?text=Ve-Ge+Kup+Not", CategoryId = 7, BrandId = 23 },
            new Product { Id = 62, Name = "Ve-Ge Aydınger Kağıdı A4", Description = "Çizim İçin", Price = 120m, StockQuantity = 100, ImageUrl = "https://placehold.co/400x400?text=Ve-Ge+Aydinger", CategoryId = 7, BrandId = 23 },

            // === 8. ÇANTA & BESLENME ===
            new Product { Id = 63, Name = "Eastpak Padded Pak'r Siyah", Description = "Klasik Sırt Çantası", Price = 1850m, StockQuantity = 40, ImageUrl = "https://placehold.co/400x400?text=Eastpak+Padded", CategoryId = 8, BrandId = 24 },
            new Product { Id = 64, Name = "Eastpak Benchmark Kalemlik", Description = "Tek Bölmeli", Price = 450m, StockQuantity = 60, ImageUrl = "https://placehold.co/400x400?text=Eastpak+Benchmark", CategoryId = 8, BrandId = 24 },
            new Product { Id = 65, Name = "Adel Ortopedik İlkokul Çantası", Description = "Sırt Destekli", Price = 750m, StockQuantity = 50, ImageUrl = "https://placehold.co/400x400?text=Adel+Canta", CategoryId = 8, BrandId = 4 },
            new Product { Id = 66, Name = "Adel 3 Bölmeli Çelik Beslenme Kabı", Description = "Kilitli", Price = 320m, StockQuantity = 80, ImageUrl = "https://placehold.co/400x400?text=Adel+Beslenme+Kabi", CategoryId = 8, BrandId = 4 },
            new Product { Id = 67, Name = "Maped Picnik Concept Suluk 430ml", Description = "BPA İçermez", Price = 295m, StockQuantity = 100, ImageUrl = "https://placehold.co/400x400?text=Maped+Suluk", CategoryId = 8, BrandId = 18 },
            new Product { Id = 68, Name = "Maped Çift Katlı Beslenme Kutusu", Description = "Mikrodalgaya Girebilir", Price = 350m, StockQuantity = 75, ImageUrl = "https://placehold.co/400x400?text=Maped+Beslenme+Kutusu", CategoryId = 8, BrandId = 18 },

            // === 9. DOSYALAMA ===
            new Product { Id = 69, Name = "Noki A4 Geniş Klasör Mavi", Description = "Çelik Mekanizmalı", Price = 85m, StockQuantity = 300, ImageUrl = "https://placehold.co/400x400?text=Noki+Klasor", CategoryId = 9, BrandId = 25 },
            new Product { Id = 70, Name = "Noki 100'lü Şeffaf Poşet Dosya", Description = "A4 Uyumlu Föy", Price = 95m, StockQuantity = 500, ImageUrl = "https://placehold.co/400x400?text=Noki+Poset+Dosya", CategoryId = 9, BrandId = 25 },
            new Product { Id = 71, Name = "MAS Çıtçıtlı Zarf Dosya 10'lu", Description = "Şeffaf Evrak Taşıma", Price = 75m, StockQuantity = 400, ImageUrl = "https://placehold.co/400x400?text=MAS+Citcitli+Dosya", CategoryId = 9, BrandId = 17 },
            new Product { Id = 72, Name = "MAS Telli Dosya 50'li Paket", Description = "Plastik", Price = 150m, StockQuantity = 200, ImageUrl = "https://placehold.co/400x400?text=MAS+Telli+Dosya", CategoryId = 9, BrandId = 17 },
            new Product { Id = 73, Name = "Gıpta Sunum Dosyası 40 Yaprak", Description = "Sabit Föylü", Price = 95m, StockQuantity = 150, ImageUrl = "https://placehold.co/400x400?text=Gipta+Sunum+Dosyasi", CategoryId = 9, BrandId = 6 },
            new Product { Id = 74, Name = "Gıpta Körüklü Evrak Çantası", Description = "13 Bölmeli", Price = 220m, StockQuantity = 90, ImageUrl = "https://placehold.co/400x400?text=Gipta+Evrak+Cantasi", CategoryId = 9, BrandId = 6 },
            new Product { Id = 75, Name = "Esselte Askılı Dosya 25'li", Description = "Çekmece İçi", Price = 450m, StockQuantity = 40, ImageUrl = "https://placehold.co/400x400?text=Esselte+Askili+Dosya", CategoryId = 9, BrandId = 26 },
            new Product { Id = 76, Name = "Esselte Dar Klasör Siyah", Description = "Karton Kapak", Price = 80m, StockQuantity = 120, ImageUrl = "https://placehold.co/400x400?text=Esselte+Dar+Klasor", CategoryId = 9, BrandId = 26 }
        );
    }
}
