# DecentM.Collections

This package is a set of common collection reimplementations using only interfaces available in Udon.

Each collection extends from a common Collection base class, and each implements a ToArray and FromArray function, so conversion between them is easy.

All collections can be used the same way. For example, if you need a `List`:
- On the same GameObject that has your script, add a List
- In your script:
  - Add `use DecentM.Collections;` on top
  - Add `private List myList;` to your class
  - In your class, add this to the Awake, or Start function: `this.myList = GetComponent<List>();`

After these setup steps, you can use the basic List interface.

## Construction

It's not possible to call the constructor of any collection class, as they all extend from UdonSharpBehaviour. Instead, each time you need a collection, you must add a serialised field (public or private) to your class.
If you need to get a new list and don't need the current values anymore, you can call `.Clear()` on any collection to reset it to an empty value.

## Generics

While Generics aren't supported, the backing data type for collections is `object`, so you can cast the return value of functions to the same type as you inserted them.

## List example

```cs
public class MyClass extends UdonSharpBehaviour
{
	private List myList;

	void Start()
	{
		this.myList = GetComponent<MyList>();
	}

	void HelloWorld(string input)
	{
		this.myList.Add("Hello, world!");
		this.myList.AddRange(new object[] { "My", "name", "is", "me" });

		bool hasName = this.myList.Contains("name"); // true
		string item2 = (string)this.myList.ElementAt(1); // "My"

		string[] all = (string[])this.myList.ToArray(); // new string[] { "Hello, world!", "My", "name", "is", "me" }

		this.myList.Clear();
	}
}
```
