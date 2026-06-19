# Reflection정의 

런타임 시점에서 타입(클래스,메서드 속성 등)의 메타데이터를 분석하고,동적으로 객체를 생성하거나 메서드를 호출하는 기술.

컴파일 타임에 결정된 타입에 의존하지 않고, 런타임에 타입을 조회하여 유연한 프로그래밍을 가능하게 한다.

# Reflection 장점(사용 이유)
- 유연성 및 확장성 : 코드 수정 없이 외부 설정 파일이나 데이터만 변경하여 기능을 추가/제거할 수 있다.

- 자동화 : 반복적인 작업을 자동화 할 수 있다.
    - 예 : Json직렬화 라이브러리,유니티의 인스펙터 시스템


# Reflection 단점
- 성능 오버헤드: 리플렉션은 타입 검색, 멤버 접근 시 많은 연산이 필요합니다. 컴파일된 직접 호출보다 훨씬 느립니다.

- 타입 안전성 저하: 런타임에 이름을 문자열(string)로 찾기 때문에, 오타가 나거나 리팩토링 시 변수명이 바뀌면 컴파일 타임에 에러를 잡을 수 없고 실행 중 에러가 발생합니다.

- 캡슐화 파괴: private 멤버에 강제로 접근할 수 있어 객체지향의 캡슐화 원칙을 깰 수 있습니다.

# Reflection 사용법

```Csharp
// 1. 타입 정보 얻기 (설계도 확보)
    Type type = obj.GetType();
```
```Csharp
   // 2. 클래스 내부의 모든 필드(멤버 변수) 정보를 가져옵니다.
   // BindingFlags를 사용해 public/private 등 범위를 지정할 수 있습니다.
   FieldInfo[] fields = data.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

   foreach (var field in fields)
   {
       // 3. 필드 이름과 현재 가지고 있는 값을 출력합니다.
       Debug.Log($"필드 이름: {field.Name}, 값: {field.GetValue(player)}");
   }
```