import { create } from 'zustand';

export type ToastType = 'error' | 'success' | 'info';

interface ToastState {
	message: string | null;
	type: ToastType;
	showToast: (message: string, type: ToastType, duration?: number) => void;
	clearToast: () => void;
}

export const useToastStore = create<ToastState>((set) => ({
	message: null,
	type: 'error',
	showToast: (message: string, type: ToastType, duration = 5000) => {
		set({ message, type });

		// Clear the toast after the specified duration
		const timeoutId = setTimeout(() => {
			set({ message: null });
		}, duration);

		// Return cleanup function in case we need to cancel the timeout
		return () => clearTimeout(timeoutId);
	},
	clearToast: () => set({ message: null }),
}));
