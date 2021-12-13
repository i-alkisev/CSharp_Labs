#pragma once
#include "mkl.h"

extern "C"  _declspec(dllexport)
void VM_Interpolate(MKL_INT nx, double* x, MKL_INT ny, double* y, int nsite, double* y_new, int& ret);