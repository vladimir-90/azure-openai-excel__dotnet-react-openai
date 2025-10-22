import { useToastStore } from '../../stores/toastStore';

function Toast() {
	const toasts = useToastStore((state) => state.toasts);
	const removeToast = useToastStore((state) => state.removeToast);
	const prolongToast = useToastStore((state) => state.prolongToast);

	return (
		<div className="toast-container">
			{toasts.map((toast) => (
				<div
					key={toast.id}
					className={`toast-notification toast-${toast.type}`}
					onClick={() => prolongToast(toast.id)}
					style={{ cursor: 'pointer' }}
				>
					<div className="toast-content">
						{/* <span className="toast-icon">
							{toast.type === 'error' && '⚠️'}
							{toast.type === 'success' && '✓'}
							{toast.type === 'info' && 'ℹ️'}
						</span> */}
						<span className="toast-message">{toast.message}</span>
					</div>
					<button
						className="toast-close"
						onClick={(e) => {
							e.stopPropagation();
							removeToast(toast.id);
						}}
						aria-label="Close notification"
					>
						✕
					</button>
				</div>
			))}
		</div>
	);
}

export default Toast;
