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

```Csharp

```
### 메소드
```Csharp
1.GetType()
객체의 타입 정보를 가져옵니다.

Player player = new Player();

Type type = player.GetType();

Debug.Log(type.Name); // Player
```

```Csharp
2.typeof()
컴파일 시점에 타입 정보를 가져옵니다.

Type type = typeof(Player);

Debug.Log(type.Name);
```

```Csharp
3. GetFields()
필드(Field) 목록을 가져옵니다.

FieldInfo[] fields = typeof(Player).GetFields();

foreach (FieldInfo field in fields)
{
    Debug.Log(field.Name);
}
```

```Csharp
4. GetField()
특정 필드 하나를 가져옵니다.

FieldInfo field = typeof(Player).GetField("hp");
```
```Csharp
5. GetProperties()
프로퍼티(Property)를 가져옵니다.

PropertyInfo[] properties =
    typeof(Player).GetProperties();

foreach (PropertyInfo property in properties)
{
    Debug.Log(property.Name);
}
```

```Csharp
6. GetProperty()
특정 프로퍼티를 가져옵니다.

PropertyInfo property =
    typeof(Player).GetProperty("Hp");
```

```Csharp
7. GetMethods()
메서드 목록을 가져옵니다.

MethodInfo[] methods =
    typeof(Player).GetMethods();

foreach (MethodInfo method in methods)
{
    Debug.Log(method.Name);
}
```

```Csharp
8. GetMethod()
특정 메서드를 가져옵니다.

MethodInfo method =
    typeof(Player).GetMethod("Attack");
```


```Csharp
9. GetMembers()
필드, 프로퍼티, 메서드를 모두 가져옵니다.

MemberInfo[] members =
    typeof(Player).GetMembers();

foreach (MemberInfo member in members)
{
    Debug.Log(member.Name);
}
```
```Csharp
10. BindingFlags
private, static 등을 포함해서 가져올 때 사용합니다.

FieldInfo[] fields =
    typeof(Player).GetFields(
        BindingFlags.Public |
        BindingFlags.NonPublic |
        BindingFlags.Instance);
```
```Csharp
11. Activator.CreateInstance()
문자열이나 Type으로 객체 생성

Type type = typeof(Player);

object obj = Activator.CreateInstance(type);

캐스팅

Player player =
    (Player)Activator.CreateInstance(type);
```