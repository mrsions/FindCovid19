# FCV19 (Find Vaccine Covid-19)

네이버 Graph API를 사용하여 잔여백신이 일어난 병원의 백신예약 페이지를 팝업해주는 프로그램입니다.
아직 이 프로그램은 검증되지 않았습니다. 혹시나 이 앱을 사용하여 백신예약에 성공하신 분이 계시다면 Issues 란 혹은 mrsions@gmail.com 으로 보고해주시면 감사하겠습니다.

![GitHub](https://img.shields.io/badge/-.NET%20Framework%204.5-512BD4?style=float-square&logo=.NET)
![GitHub](https://img.shields.io/badge/-LitJson-000000?style=float-square&logo=JSON)

<hr>

# 주의사항

![GitHub](https://img.shields.io/badge/-주의사항-F40D12?style=float-square&logo=AdBlock)
이 앱은 네이버 Graph API의 공개되지 않은 부분을 사용하며, 학습목적 이외의 용도로 사용했을 경우의 형사, 민사상을 포함한 모든 법적인 문제는 사용자에게 있습니다.

<hr>

# 빌드방법

1. VisualStudio Community 2019 버전 설치
2. .Net Framework 4.5 설치 (범용성을 위해 4.5로 사용)
3. FindCovid19.sln 파일 실행
4. 빌드&실행

# 사용방법

1. [FVC19.exe](https://github.com/mrsions/FindCovid19/releases) 파일을 실행한다.
2. 사용할 브라우저를 선택한다.
3. 검색할 위치를 지정한다. (지도상의 범위를 키우면 넓게 검색됨. 즉, 많은 병원을 찾을 수 있음)
4. 기다린다
5. 백신이 검색되면 선택한 브라우저로 백신예약창이 띄워진다.

<hr>

# 참고

1. LitJson 소스코드 포함 ( https://github.com/LitJSON/litjson )
2. 네이버 플레이스 (https://m.place.naver.com/rest/vaccine?vaccineFilter=used)

# 라이센스
![GitHub](https://img.shields.io/badge/License-GPL-green?style=float-square)
1. GPL 기반 라이센스
2. 교육용 외의 사용을 금지.
4. 불법 용도 사용/배포 금지.
