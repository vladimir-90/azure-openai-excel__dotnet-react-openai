import { useToastStore } from '../../stores/toastStore';

function Toast() {
	const message = useToastStore((state) => state.message);
	const type = useToastStore((state) => state.type);
	const clearToast = useToastStore((state) => state.clearToast);

	if (!message) {
		return null;
	}

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
				onClick={clearToast}
				aria-label="Close notification"
			>
				✕
			</button>
		</div>
	);
}

export default Toast;
