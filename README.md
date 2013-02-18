SingleGlobalLock
================

Explanation
-----------
I needed a Mutex that spanned multiple instances of the same application within the same machine.
This fulfilled that need and made implementation VERY easy.

Example
-------

    using (new SingleGlobalInstance(5000)) // 5 Seconds
    {
        // Your code goes here
    }


Credits 
-------
- http://stackoverflow.com/q/229565/1677
- http://stackoverflow.com/a/7810107/1677
