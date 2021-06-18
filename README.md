# FCV19 (Find Vaccine Covid-19)

웹 자동화 플러그인 셀레니움과 네이버 Graph API를 사용하여 잔여백신이 일어난 병원의 백신예약을 진행해주는는 프로그램입니다.
단순히 팝업만을 원하시면 [v1.0.1.0 버전](https://github.com/mrsions/FindCovid19/releases/tag/v1.0.1.0)을 사용하시면됩니다.

아직 이 프로그램은 검증되지 않았습니다. 혹시나 이 앱을 사용하여 백신예약에 성공하신 분이 계시다면 Issues 란 혹은 mrsions@gmail.com 으로 보고해주시면 감사하겠습니다.

![GitHub](https://img.shields.io/badge/-.NET%20Framework%204.5-512BD4?style=float-square&logo=.NET)
![GitHub](https://img.shields.io/badge/-LitJson-000000?style=float-square&logo=JSON)
![GitHub](https://img.shields.io/badge/-Chrome-7F0000?style=float-square&logo=Chrome%20Browser)

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

1. [FVC19.exe](https://github.com/mrsions/FindCovid19/releases) 파일을  실행한다.
2. 네이버 로그인을 한다.
3. (창이 표시될 경우) 본인인증을 한다.
6. 검색할 위치를 지정한다. (지도상의 범위를 키우면 넓게 검색됨. 즉, 많은 병원을 찾을 수 있음)
7. [현 지도에서 검색]을 누른다.
8. 팝업을 뛰울 백신을 선택한다 
9. [시작]을 누르면 백신 검색이 시작된다.
10. 백신 예약이 완료되면 프로그램이 일시정지한다. 
11. 엔터를 눌러서 다시 시작할 수 있다. (오작동으로 예약성공이라고 표기된 경우)

※ 크롬 브라우저가 설치 돼 있어야한다.<br>
※ 백신선택은 예약할 대상을 고를때만 사용되며, 예약시에 세부선택을 대신해주지는 않는다. (한마디로 예약창의 처음 선택돼있는 백신이 신청된다)<br>
※ (세부선택 기능이 빠져있는 관계로) 특정 백신을 선택 해제 했다고 해서, 꼭 그 백신이 예약이 안된다는것을 보장하진 않는다. (다른 백신으로 예약될 수도 있으니 꼭 확인하자)<br>
※ (세부선택 기능이 빠져있는 관계로) 반대로 특정 백신만 선택했다고 해서, 꼭 그 백신이 예약되는것도 보장하지 않는다. <br>

<hr>

# 참고

1. LitJson ( https://github.com/LitJSON/litjson )
1. Selenium
2. 네이버 플레이스 (https://m.place.naver.com/rest/vaccine)

# 라이센스
![GitHub](https://img.shields.io/badge/License-GPL-green?style=float-square)
1. GPL 기반 라이센스
2. 교육용 외의 사용을 금지.
4. 불법 용도 사용/배포 금지.
