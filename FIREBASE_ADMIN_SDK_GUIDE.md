# Firebase Admin SDK Implementation Guide

## Overview

This implementation uses Firebase Admin SDK to generate proper download tokens for Firebase Storage files, eliminating the need to manually create access tokens in the Firebase Console.

## ✅ **What's Changed**

### 1. **Package Added**
- `FirebaseAdmin` version 3.0.1 has been added to the Infrastructure project

### 2. **Firebase Integration**
- **Automatic Token Generation**: Every uploaded file now gets a unique download token
- **Firebase-Style URLs**: Generated URLs match Firebase's native format with `token=` parameter
- **Backward Compatibility**: Existing files without tokens can be updated

### 3. **Unified Upload API**
- **Single Endpoint**: One `/api/v1/files/upload` endpoint handles all file types
- **Folder-Based**: Different file types are handled by specifying different folders
- **Smart Detection**: Automatically detects image files and applies appropriate processing

## 🚀 **How to Use**

### **1. Upload Files to Different Folders**

```bash
# Upload a document
POST /api/v1/files/upload?folder=uploads&fileType=Document
Content-Type: multipart/form-data

# Upload an image
POST /api/v1/files/upload?folder=images&fileType=Image
Content-Type: multipart/form-data

# Upload an avatar (automatically processed as image)
POST /api/v1/files/upload?folder=avatars&fileType=Avatar
Content-Type: multipart/form-data

# Upload company logo (automatically processed as image)
POST /api/v1/files/upload?folder=company-logos&fileType=CompanyLogo
Content-Type: multipart/form-data

# Upload job attachment
POST /api/v1/files/upload?folder=job-attachments&fileType=JobAttachment
Content-Type: multipart/form-data

Response:
{
  "success": true,
  "fileUrl": "https://firebasestorage.googleapis.com/v0/b/gigbuds-82cee.firebasestorage.app/o/avatars%2Fprofile_1748694346_3be72376.jpg?alt=media&token=abc123-def456-ghi789",
  "fileName": "profile_1748694346_3be72376.jpg",
  "filePath": "avatars/profile_1748694346_3be72376.jpg",
  "fileSize": 12345,
  "contentType": "image/jpeg"
}
```

### **2. Upload Multiple Files**

```bash
# Upload multiple files to the same folder
POST /api/v1/files/upload-multiple?folder=documents&fileType=Document
Content-Type: multipart/form-data

Response:
[
  {
    "success": true,
    "fileUrl": "https://firebasestorage.googleapis.com/v0/b/gigbuds-82cee.firebasestorage.app/o/documents%2Ffile1.pdf?alt=media&token=token1",
    "fileName": "file1.pdf",
    // ... more properties
  },
  {
    "success": true,
    "fileUrl": "https://firebasestorage.googleapis.com/v0/b/gigbuds-82cee.firebasestorage.app/o/documents%2Ffile2.pdf?alt=media&token=token2",
    "fileName": "file2.pdf",
    // ... more properties
  }
]
```

### **3. Supported Folder Types**

| Folder | Purpose | Auto-Processing |
|--------|---------|----------------|
| `uploads` | General files | Document handling |
| `images` | General images | Image processing |
| `avatars` | User profile pictures | Image processing |
| `company-logos` | Company branding | Image processing |
| `job-attachments` | Job-related files | Document handling |
| `documents` | Business documents | Document handling |

### **4. File Type Detection**

The API automatically detects and processes files based on:
1. **Content-Type**: `image/*` files are processed as images
2. **Folder Name**: Files uploaded to `avatars`, `company-logos`, or `images` folders are processed as images
3. **File Extension**: Validated against allowed extensions in configuration

### **5. Fix Existing Files Without Tokens**

```bash
# Add token to existing file
POST /api/v1/files/add-token
Content-Type: application/json

{
  "uploads/existing-file.pdf"
}

Response:
{
  "message": "Download token added successfully",
  "downloadUrl": "https://firebasestorage.googleapis.com/v0/b/gigbuds-82cee.firebasestorage.app/o/uploads%2Fexisting-file.pdf?alt=media&token=new-token-123"
}
```

### **6. Get Download URL for Any File**

```bash
# Get download URL
GET /api/v1/files/download-url?filePath=uploads/file.pdf

Response:
{
  "downloadUrl": "https://firebasestorage.googleapis.com/v0/b/gigbuds-82cee.firebasestorage.app/o/uploads%2Ffile.pdf?alt=media&token=token-123"
}
```

### **7. Generate Firebase Auth Tokens (Optional)**

```bash
# Generate custom auth token for user
POST /api/v1/files/generate-auth-token
Content-Type: application/json

{
  "uid": "user123",
  "customClaims": {
    "role": "premium",
    "subscription": "active"
  }
}

Response:
{
  "customToken": "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
  "uid": "user123",
  "expiresIn": 3600
}
```

## 🔧 **Configuration**

Your `appsettings.Development.json` should have:

```json
{
  "Firebase": {
    "ProjectId": "gigbuds-82cee",
    "StorageBucket": "gigbuds-82cee.firebasestorage.app",
    "ServiceAccountKeyPath": "E:\\path\\to\\firebase-service.json"
  }
}
```

## 🌐 **Frontend Integration**

The generated URLs work immediately in any context:

### **Direct HTML**
```html
<!-- Images -->
<img src="https://firebasestorage.googleapis.com/v0/b/gigbuds-82cee.firebasestorage.app/o/images%2Fphoto.jpg?alt=media&token=abc123" />

<!-- Download Links -->
<a href="https://firebasestorage.googleapis.com/v0/b/gigbuds-82cee.firebasestorage.app/o/documents%2Ffile.pdf?alt=media&token=def456" download>Download PDF</a>
```

### **JavaScript/React**
```javascript
// Fetch file
const response = await fetch(downloadUrl);
const blob = await response.blob();

// Display image
<img src={downloadUrl} alt="Uploaded file" />

// Download file
window.open(downloadUrl, '_blank');
```

### **Mobile Apps**
```dart
// Flutter example
Image.network(downloadUrl)

// React Native
<Image source={{uri: downloadUrl}} />
```

## 🔐 **Security**

### **Public Access**
- Generated URLs work without authentication
- Tokens are unique and hard to guess
- No expiration by default (like Firebase Web SDK)

### **Private Access (Optional)**
- Use `GenerateCustomTokenAsync()` for user-specific tokens
- Implement custom security rules in Firebase Console
- Tokens can include custom claims for authorization

## 🛠 **Migration for Existing Files**

If you have existing files without tokens, use this utility:

```csharp
// In your service or controller
public async Task MigrateExistingFiles()
{
    var existingFiles = new[] {
        "uploads/old-file-1.pdf",
        "uploads/old-file-2.jpg",
        "images/old-image.png"
    };

    foreach (var filePath in existingFiles)
    {
        try
        {
            var newUrl = await _fileStorageService.AddDownloadTokenToExistingFile(filePath);
            Console.WriteLine($"Updated {filePath}: {newUrl}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update {filePath}: {ex.Message}");
        }
    }
}
```

## ✨ **Benefits**

1. **No Manual Token Management**: Tokens are generated automatically
2. **Firebase Compatibility**: URLs match Firebase Web SDK format exactly
3. **Immediate Access**: Files are accessible right after upload
4. **Cross-Platform**: Works on web, mobile, and desktop
5. **Scalable**: No need for server-side URL generation for file access

## 🔍 **Troubleshooting**

### **"File not found" errors**
- Check if the file path is correct
- Ensure the file was uploaded successfully
- Verify Firebase credentials are configured

### **"Access denied" errors**
- Check Firebase Storage security rules
- Ensure the download token is valid
- Verify the bucket permissions

### **Firebase Admin SDK initialization errors**
- Ensure `firebase-service.json` exists at the specified path
- Check that the service account has Storage Admin permissions
- Verify the `ProjectId` in configuration matches your Firebase project

---

**Your files now work exactly like Firebase Web SDK's `getDownloadURL()` method! 🚀** 