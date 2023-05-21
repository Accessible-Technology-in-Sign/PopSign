// Copyright © 2018 Unity Technologies. All rights reserved.

int PlayerMain(int argc, const char *argv[]);

#if UNITY_ASAN
extern "C"
{
    extern void unity_asan_configure();
}
#endif

int main(int argc, const char *argv[])
{
#if UNITY_ASAN
    unity_asan_configure();
#endif

    return PlayerMain(argc, argv);
}
