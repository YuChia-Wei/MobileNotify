# MobileNotify POC

這是一個以 ASP.NET Core Minimal API 實作的 Push Notification 測試專案，支援：

- Apple VoIP Push（APNs）
- Firebase Cloud Messaging（FCM）

專案核心目的

1. 測試利用 Azure Key Vault 或本機憑證 動態載入 APNs 推播的憑證
2. 測試使用 APNs / FCM API 來推播訊息。

> [!WARNING]
> 此專案用於測試手機設備的推播，並已移除憑證與認證資訊，目前版本尚未進行完整測試，僅供開發參考

---

## 功能

- .NET 9.0 + C# 13.0 開發
- Minimal API 架構，輕量易擴充
- APNs VoIP Push
    - 透過 Azure Key Vault 或本機檔案載入 `.p12` 憑證
    - 支援發送到開發/正式伺服器
- Firebase Cloud Messaging
    - 使用 Firebase Admin SDK
    - 自訂 payload 資料與標題／內文
- Swagger UI 文件支援（僅開發環境顯示）

---

## API 使用說明

### 1. Apple VoIP Push

POST /apns/voip/{deviceToken}

- Path 參數
  - `deviceToken`：目標裝置 VoIP push token
- 回傳：APNs 回應結果物件（JSON）

### 2. Firebase Cloud Messaging

POST /fcm/{deviceToken}
- Path 參數
  - `deviceToken`：目標裝置 push token
- Body：無（範例 payload 內建 traceId、標題、內文）
- 回傳：200 OK

---

## 設定說明

- ApnsSettingOptions
  - `VoipCertificateName`、`VoipCertificatePassword`：憑證存取資訊
  - `SendToDevelopServer`：是否發送到開發伺服器
- AzureKeyVaultSettingOptions
  - Key Vault 存取 URI、Service Principal 資訊
- Firebase
  - `CredentialsPath`：指向 Firebase 服務帳戶金鑰（JSON）

> 憑證來源可透過 DI 決定：
> - Azure Key Vault
> - Local File