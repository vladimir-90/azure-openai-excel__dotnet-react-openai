import AIRequestCost from './base/AIRequestCost';
import type { OpenAIQueryCost } from '../services/excel-analysis.service';

interface ChatMessageProps {
	text: string;
	sender: 'you' | 'sys' | 'ai';
	cost?: OpenAIQueryCost | null;
}

function ChatMessage({ text, sender, cost }: ChatMessageProps) {
	const avatarCss =
		sender === 'you'
			? 'user-avatar'
			: sender === 'sys'
			? 'system-avatar'
			: sender === 'ai'
			? 'ai-avatar'
			: '';
	const textCss =
		sender === 'you'
			? 'text-white user-message-bubble'
			: sender === 'sys'
			? 'text-dark system-message-bubble'
			: sender === 'ai'
			? 'text-white ai-message-bubble'
			: '';
	const alignRight = sender === 'you' || sender === 'ai';

	return (
		<div className="mb-3 message-bubble">
			<div
				className={`d-flex gap-2 align-items-end ${
					alignRight && 'flex-row-reverse'
				}`}
			>
				<div className="flex-shrink-0">
					<div
						className={`rounded-circle d-flex align-items-center justify-content-center text-white fw-bold avatar-float ${avatarCss}`}
					>
						{sender.toUpperCase()}
					</div>
				</div>
				<div className="message-width position-relative">
					<div className={`p-3 position-relative ${textCss}`}>
						<div className="message-text">{text}</div>
						{!!cost && sender === 'ai' && (
							<AIRequestCost cost={cost} />
						)}
					</div>
				</div>
			</div>
		</div>
	);
}

export default ChatMessage;
