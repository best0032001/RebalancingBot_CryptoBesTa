# RebalancingBot_CryptoBesTa
RebalancingBot_CryptoBesTa เป็น Project Open source สำหรับ การพัฒนา Bot เทรด Crypto บน Bitkub Exchange ตัว Project พัฒนาด้วย .NET5 การติดตั้ง จะใช้ Docker compose เป็น tools สำหรับ Run Container BOT Service

**feature**
V 1.0
1 ตั้งเวลาBotที่ Timer ที่อยู่ใน BackgroundService 
2 แจ้งเตือนยอดรวมไปที่ Line Notify

**Tech Stack**

1 Dockercompose

2 .NET

**BOT Time Config**
สามารถตั้งเวลาให้ Bot ทำงานที่ Timer 
ที่อยู่ใน BackgroundService 
ค่า เริ่มต้นคือ ทุก 10 วินาที


**Deploy**
1 add env file ก่อน Run Docker-compose up -d --build
2 Edit  env file กำหนดค่า 3 ตัว
API_KEY=xxx
API_SECRET=xxx
LINE_TOKEN=xxx
3 Run Docker-compose up -d --build

