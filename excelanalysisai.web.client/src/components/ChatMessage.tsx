import AIRequestCost from './base/AIRequestCost';
import type { OpenAIQueryCost } from '../services/excel-analysis.service';
import { useState, useRef, useLayoutEffect } from 'react';

const MAX_MESSAGE_HEIGHT = 250;

interface ChatMessageProps {
	text: string;
	sender: 'you' | 'sys' | 'ai';
	cost?: OpenAIQueryCost | null;
}

function ChatMessage({ text, sender, cost }: ChatMessageProps) {
	const [isExpanded, setIsExpanded] = useState(false);
	const [isCopied, setIsCopied] = useState(false);
	const [isOverflowed, setIsOverflowed] = useState(false);
	const contentRef = useRef<HTMLDivElement>(null);

	useLayoutEffect(() => {
		const checkOverflow = () => {
			if (contentRef.current) {
				const isOverflow =
					contentRef.current.scrollHeight > MAX_MESSAGE_HEIGHT;
				setIsOverflowed(isOverflow);
			}
		};

		checkOverflow();
		const timer = setTimeout(checkOverflow, 0);
		return () => clearTimeout(timer);
	}, [text]);

	const handleCopy = async () => {
		await navigator.clipboard.writeText(text);
		setIsCopied(true);
		setTimeout(() => setIsCopied(false), 2000);
	};

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
					<div
						ref={contentRef}
						className={`p-3 position-relative ${textCss}`}
						style={{
							maxHeight: isExpanded
								? 'none'
								: `${MAX_MESSAGE_HEIGHT}px`,
							overflow: isExpanded ? 'visible' : 'hidden',
							transition: 'max-height 0.3s ease',
						}}
					>
						<div className="message-text">{text}</div>
					</div>

					<div className="message-footer">
						{!!cost && sender === 'ai' && (
							<AIRequestCost cost={cost} variant="compact" />
						)}
						<div className="message-actions">
							<button
								onClick={handleCopy}
								className="message-action-button copy-button"
							>
								{isCopied ? '✓ Copied' : '📋 Copy'}
							</button>
							{isOverflowed && (
								<button
									onClick={() => setIsExpanded(!isExpanded)}
									className="message-action-button expand-button"
								>
									{isExpanded ? '▲ Collapse' : '▼ Expand'}
								</button>
							)}
						</div>
					</div>
				</div>
			</div>
		</div>
	);
}

export default ChatMessage;
