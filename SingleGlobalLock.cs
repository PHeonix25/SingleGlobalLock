namespace PHeonix25.Utils
{

    /// <summary>
      /// Lifted directly from http://stackoverflow.com/q/229565/1677 - specifically: http://stackoverflow.com/a/7810107/1677
    /// </summary>
    class SingleGlobalInstance : IDisposable
    {
        public bool hasHandle = false;
        Mutex mutex;

        private void InitMutex()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string mutexId = string.Format("Global\\{{{0}}}", appGuid);
            mutex = new Mutex(false, mutexId);

            var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
            var securitySettings = new MutexSecurity();
            securitySettings.AddAccessRule(allowEveryoneRule);
            mutex.SetAccessControl(securitySettings);
        }

        public SingleGlobalInstance(int millisecondsTimeout)
        {
            InitMutex();
            try
            {
                if (millisecondsTimeout <= 0)
                    hasHandle = mutex.WaitOne(Timeout.Infinite, false);
                else
                    hasHandle = mutex.WaitOne(millisecondsTimeout, false);

                if (hasHandle == false)
                    throw new TimeoutException("Timeout waiting for exclusive access on SingleGlobalInstance");
            }
            catch (AbandonedMutexException)
            {
                hasHandle = true;
            }
        }

        public void Dispose()
        {
            if (mutex != null)
            {
                if (hasHandle)
                    mutex.ReleaseMutex();
                mutex.Dispose();
            }
        }
    }
}
