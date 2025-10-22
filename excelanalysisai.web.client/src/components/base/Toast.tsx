import { useEffect } from 'react';

interface ToastProps {
	message: string;
	type?: 'error' | 'success' | 'info';
	onClose: () => void;
	duration?: number;
}

function Toast({
	message,
	type = 'error',
	onClose,
	duration = 10000,
}: ToastProps) {
	useEffect(() => {
		const timer = setTimeout(onClose, duration);
		return () => clearTimeout(timer);
	}, [onClose, duration]);

	return (
		<div className={`toast-notification toast-${type}`}>
			<div className="toast-content">
				{/* <span className="toast-icon">
					{type === 'error' && '⚠️'}
					{type === 'success' && '✓'}
					{type === 'info' && 'ℹ️'}
				</span> */}
				<span className="toast-message">{message}</span>
			</div>
			<button
				className="toast-close"
				onClick={onClose}
				aria-label="Close notification"
			>
				✕
			</button>
		</div>
	);
}

export default Toast;
