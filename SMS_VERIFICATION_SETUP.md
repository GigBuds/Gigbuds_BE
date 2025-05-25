# SMS Verification Setup with SpeedSMS and Redis

This document explains how to set up and use SMS verification in the GigBuds application using SpeedSMS API and Redis for storing verification codes.

## Overview

The SMS verification system consists of:
- **SpeedSMS API**: For sending SMS messages
- **Redis**: For storing verification codes temporarily
- **Verification Flow**: Automatic SMS sending after registration + manual verification endpoints

## Prerequisites

1. **Redis Server**: Install and run Redis locally or use a cloud Redis service
2. **SpeedSMS Account**: Sign up at [SpeedSMS](https://speedsms.vn/) and get your API credentials
3. **SpeedSMS Gateway App**: Download the Android app for TYPE_GATEWAY (Type 5) usage

## Configuration

### 1. Redis Configuration

Update `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "RedisDb": "localhost:6379"
  },
  "Redis": {
    "ConnectionString": "localhost:6379",
    "VerificationCodeExpirationMinutes": 5
  }
}
```

### 2. SpeedSMS Configuration

Update `appsettings.Development.json`:

```json
{
  "SpeedSMS": {
    "AccessToken": "your-speedsms-access-token",
    "Type": 5,
    "DeviceId": "your-device-id"
  }
}
```

**SpeedSMS Types:**
- `1`: TYPE_QC (Advertising)
- `2`: TYPE_CSKH (Customer Service)
- `3`: TYPE_BRANDNAME (Brand Name)
- `4`: TYPE_BRANDNAME_NOTIFY (Brand Name Notification)
- `5`: TYPE_GATEWAY (Personal Phone Gateway - Recommended for development)

## Installation

### 1. Install Redis

**Windows:**
```bash
# Using Chocolatey
choco install redis-64

# Or download from: https://github.com/microsoftarchive/redis/releases
```

**macOS:**
```bash
brew install redis
brew services start redis
```

**Linux:**
```bash
sudo apt update
sudo apt install redis-server
sudo systemctl start redis-server
```

### 2. Start Redis Server

```bash
redis-server
```

Verify Redis is running:
```bash
redis-cli ping
# Should return: PONG
```

## API Endpoints

### 1. Send Verification Code

**Endpoint:** `POST /api/identityapi/send-verification-code`

**Request Body:**
```json
{
  "phoneNumber": "0943911515"
}
```

**Response:**
```json
{
  "message": "Verification code sent successfully"
}
```

### 2. Verify Phone Number

**Endpoint:** `POST /api/identityapi/verify-phone`

**Request Body:**
```json
{
  "phoneNumber": "0943911515",
  "verificationCode": "123456"
}
```

**Response:**
```json
{
  "message": "Phone number verified successfully"
}
```

## Registration Flow

1. **User Registration**: User registers with phone number
2. **Automatic SMS**: System automatically sends verification code via SMS
3. **Phone Verification**: User enters verification code to activate account
4. **Account Activation**: Phone number is marked as confirmed

## Phone Number Format

The system automatically formats phone numbers:
- Input: `0943911515` → Output: `84943911515`
- Input: `943911515` → Output: `84943911515`
- Input: `84943911515` → Output: `84943911515` (no change)

## Verification Code Details

- **Length**: 6 digits
- **Expiration**: 5 minutes (configurable)
- **Storage**: Redis with automatic expiration
- **Format**: Random 6-digit number (100000-999999)

## Error Handling

### Common Errors:

1. **Redis Connection Failed**
   - Check if Redis server is running
   - Verify connection string in configuration

2. **SMS Sending Failed**
   - Check SpeedSMS credentials
   - Verify phone number format
   - Check SpeedSMS account balance

3. **Invalid Verification Code**
   - Code may have expired (5 minutes)
   - Code may have been used already
   - Wrong code entered

## Testing

### 1. Test SMS Sending

```bash
curl -X POST "https://localhost:7000/api/identityapi/send-verification-code" \
  -H "Content-Type: application/json" \
  -d '{"phoneNumber": "0943911515"}'
```

### 2. Test Phone Verification

```bash
curl -X POST "https://localhost:7000/api/identityapi/verify-phone" \
  -H "Content-Type: application/json" \
  -d '{"phoneNumber": "0943911515", "verificationCode": "123456"}'
```

### 3. Check Redis Storage

```bash
redis-cli
> KEYS verification_code:*
> GET verification_code:84943911515
> TTL verification_code:84943911515
```

## Security Considerations

1. **Rate Limiting**: Implement rate limiting for SMS sending to prevent abuse
2. **Phone Number Validation**: Validate phone number format before sending SMS
3. **Code Expiration**: Verification codes expire after 5 minutes
4. **One-Time Use**: Codes are deleted after successful verification
5. **Secure Storage**: Codes are stored in Redis, not in the database

## Monitoring and Logging

The system logs:
- SMS sending attempts and results
- Verification code generation
- Phone number verification attempts
- Redis operations

Check logs for debugging SMS issues.

## Production Considerations

1. **Redis Persistence**: Configure Redis persistence for production
2. **SMS Provider**: Consider using multiple SMS providers for redundancy
3. **Rate Limiting**: Implement proper rate limiting
4. **Monitoring**: Set up monitoring for SMS delivery rates
5. **Cost Management**: Monitor SMS usage and costs

## Troubleshooting

### Redis Issues:
```bash
# Check Redis status
redis-cli ping

# Check Redis logs
tail -f /var/log/redis/redis-server.log

# Clear all verification codes (development only)
redis-cli FLUSHDB
```

### SpeedSMS Issues:
- Verify API credentials
- Check account balance
- Test with SpeedSMS API directly
- Ensure device ID is correct for TYPE_GATEWAY

## Development Tips

1. **Local Testing**: Use a test phone number for development
2. **Mock SMS**: Create a mock SMS service for unit testing
3. **Redis GUI**: Use Redis Desktop Manager for easier Redis debugging
4. **Logging**: Enable detailed logging for SMS operations

## Support

For issues related to:
- **SpeedSMS**: Contact SpeedSMS support
- **Redis**: Check Redis documentation
- **Application**: Check application logs and error messages 