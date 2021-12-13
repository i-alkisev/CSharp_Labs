#include "pch.h"
#include "mkl.h"
#include <memory>

extern "C"  _declspec(dllexport)
void VM_Interpolate(MKL_INT nx, double* x, MKL_INT ny, double* y, int nsite, double* y_new, int &ret)
{
	ret = -1;
	try
	{
		DFTaskPtr task;
		if (dfdNewTask1D(&task,
			nx, x, DF_UNIFORM_PARTITION,
			ny, y, DF_MATRIX_STORAGE_ROWS) != DF_STATUS_OK) return;

		std::unique_ptr<double[]> scoefs(new double[nx * ny * (DF_PP_CUBIC)]);
		if (dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL,
			DF_BC_FREE_END, nullptr,
			DF_NO_IC, nullptr,
			scoefs.get(), DF_NO_HINT) != DF_STATUS_OK) return;

		if (dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD) != DF_STATUS_OK) return;

		const MKL_INT dorder[] = { 1 };
		if (dfdInterpolate1D(task,
			DF_INTERP, DF_METHOD_PP,
			nsite, x, DF_UNIFORM_PARTITION,
			1, dorder,
			nullptr,
			y_new, DF_MATRIX_STORAGE_ROWS, nullptr) != DF_STATUS_OK) return;

		if (dfDeleteTask(&task) != DF_STATUS_OK) return;

		ret = 0;
	}
	catch (...)
	{ }
}