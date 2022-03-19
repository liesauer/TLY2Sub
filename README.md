# TLY转VMESS订阅

## .NET端

.NET文件夹里的是.NET控制台客户端，部署PHP版本时不需要，请删除！

## PHP端部署
1. `composer install`
2. 将项目文件夹复制到服务器中
3. 将`.NET`文件夹删除
4. 复制`src/config.example.php`到`src/config.php`
5. 配置
6. 访问`服务器地址/项目文件夹/index.php?pass=PASS`即可

## 配置说明
```
pass：入口密码，防止入口被恶意访问
email：TLY账号
passwd：TLY密码
aes_key：tlynet923456789k
aes_iv：9987654321fedcsu
```
