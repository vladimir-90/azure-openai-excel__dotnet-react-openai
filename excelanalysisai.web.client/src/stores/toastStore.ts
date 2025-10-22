import { create } from 'zustand';

export type ToastType = 'error' | 'success' | 'info';

export interface Toast {
	id: string;
	message: string;
	type: ToastType;
}

interface ToastState {
	toasts: Toast[];
	showToast: (message: string, type: ToastType, duration?: number) => string;
	removeToast: (id: string) => void;
	prolongToast: (id: string, duration?: number) => void;
}

const timeoutMap = new Map<string, ReturnType<typeof setTimeout>>();

export const useToastStore = create<ToastState>((set) => ({
	toasts: [],
	showToast: (message: string, type: ToastType, duration = 10000) => {
		const id = `${Date.now()}-${Math.random()}`;
		const toast: Toast = { id, message, type };

		set((state) => ({
			toasts: [...state.toasts, toast],
		}));

		// Set up automatic expiration for this toast
		const timeoutId = setTimeout(() => {
			set((state) => ({
				toasts: state.toasts.filter((t) => t.id !== id),
			}));
			timeoutMap.delete(id);
		}, duration);

		// Store the timeout so it can be cancelled if needed
		timeoutMap.set(id, timeoutId);

		return id;
	},
	removeToast: (id: string) => {
		// Cancel the auto-expiration timeout if it exists
		const timeoutId = timeoutMap.get(id);
		if (timeoutId) {
			clearTimeout(timeoutId);
			timeoutMap.delete(id);
		}

		// Remove the toast from the state
		set((state) => ({
			toasts: state.toasts.filter((t) => t.id !== id),
		}));
	},
	prolongToast: (id: string, duration = 10000) => {
		// Cancel the existing timeout
		const existingTimeoutId = timeoutMap.get(id);
		if (existingTimeoutId) {
			clearTimeout(existingTimeoutId);
		}

		// Set up a new timeout with the prolonged duration
		const newTimeoutId = setTimeout(() => {
			set((state) => ({
				toasts: state.toasts.filter((t) => t.id !== id),
			}));
			timeoutMap.delete(id);
		}, duration);

		// Store the new timeout
		timeoutMap.set(id, newTimeoutId);
	},
}));
